using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Intellisense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType(Constants.ClassificationName)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class VsTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        IVsEditorAdaptersFactoryService _adaptersFactory;

        [Import]
        ICompletionBroker _completionBroker;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView view = _adaptersFactory.GetWpfTextView(textViewAdapter);
            Debug.Assert(view != null);

            CommandFilter filter = new CommandFilter(view, _completionBroker);
            IOleCommandTarget next;
            textViewAdapter.AddCommandFilter(filter, out next);
            filter.Next = next;
        }
    }

    internal sealed class CommandFilter : IOleCommandTarget
    {
        public IWpfTextView View { get; set; }
        public ICompletionBroker CompletionBroker { get; set; }
        public IOleCommandTarget Next { get; set; }
        private ICompletionSession _currentSession;

        public CommandFilter(IWpfTextView view, ICompletionBroker completionBroker)
        {
            _currentSession = null;

            View = view;
            CompletionBroker = completionBroker;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                switch ((VSConstants.VSStd2KCmdID) prgCmds[0].cmdID)
                {
                    case VSConstants.VSStd2KCmdID.AUTOCOMPLETE:
                    case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                        prgCmds[0].cmdf = (uint) OLECMDF.OLECMDF_ENABLED | (uint) OLECMDF.OLECMDF_SUPPORTED;
                        return VSConstants.S_OK;
                }
            }
            return Next.QueryStatus(pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            bool handled = false;
            int hResult = VSConstants.S_OK;

            // 1. Pre-process
            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                switch ((VSConstants.VSStd2KCmdID) nCmdID)
                {
                    case VSConstants.VSStd2KCmdID.AUTOCOMPLETE:
                    case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                        handled = StartSession();
                        break;
                    case VSConstants.VSStd2KCmdID.RETURN:
                        handled = Complete(false);
                        break;
                    case VSConstants.VSStd2KCmdID.TAB:
                        handled = Complete(true);
                        break;
                    case VSConstants.VSStd2KCmdID.CANCEL:
                        handled = Cancel();
                        break;
                }
            }

            if (!handled)
                hResult = Next.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            if (ErrorHandler.Succeeded(hResult) && pguidCmdGroup == VSConstants.VSStd2K)
            {
                switch ((VSConstants.VSStd2KCmdID) nCmdID)
                {
                    case VSConstants.VSStd2KCmdID.TYPECHAR:
                        char ch = GetTypeChar(pvaIn);
                        if (char.IsWhiteSpace(ch))
                            StartSession();
                        else if (_currentSession != null)
                            Filter();
                        break;
                    case VSConstants.VSStd2KCmdID.BACKSPACE:
                        Filter();
                        break;
                }
            }

            return hResult;
        }

        private void Filter()
        {
            if (_currentSession != null)
            {
                _currentSession.SelectedCompletionSet.SelectBestMatch();
                _currentSession.SelectedCompletionSet.Recalculate();
            }
        }

        private char GetTypeChar(IntPtr pvaIn)
        {
            return (char) (ushort) Marshal.GetObjectForNativeVariant(pvaIn);
        }

        private bool Cancel()
        {
            bool result = false;

            if (_currentSession != null)
            {
                _currentSession.Dismiss();
                result = true;
            }

            return result;
        }

        private bool Complete(bool force)
        {
            bool result = false;

            if (_currentSession != null)
            {
                if (!_currentSession.SelectedCompletionSet.SelectionStatus.IsSelected && !force)
                {
                    _currentSession.Dismiss();
                }
                else
                {
                    _currentSession.Commit();
                    result = true;
                }
            }

            return result;
        }

        private bool StartSession()
        {
            bool result = false;

            if (_currentSession == null)
            {
                SnapshotPoint caret = View.Caret.Position.BufferPosition;
                ITextSnapshot snapshot = caret.Snapshot;

                if (!CompletionBroker.IsCompletionActive(View))
                    _currentSession = CompletionBroker.CreateCompletionSession(View,
                        snapshot.CreateTrackingPoint(caret, PointTrackingMode.Positive), true);
                else
                    _currentSession = CompletionBroker.GetSessions(View)[0];

                _currentSession.Dismissed += (s, e) => _currentSession = null;

                _currentSession.Start();

                result = true;
            }

            return result;
        }
    }
}