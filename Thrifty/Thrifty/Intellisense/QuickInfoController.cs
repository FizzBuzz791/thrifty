using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Thrifty.Intellisense
{
    internal class TemplateQuickInfoController : IIntellisenseController
    {
        private ITextView _textView;
        private readonly IList<ITextBuffer> _subjectBuffers;
        private readonly TemplateQuickInfoControllerProvider _componentContext;

        private IQuickInfoSession _session;

        public TemplateQuickInfoController(ITextView textView, IList<ITextBuffer> subjectBuffers,
            TemplateQuickInfoControllerProvider templateQuickInfoControllerProvider)
        {
            _textView = textView;
            _subjectBuffers = subjectBuffers;
            _componentContext = templateQuickInfoControllerProvider;

            _textView.MouseHover += OnTextViewMouseHover;
        }

        private void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
        {
            SnapshotPoint? point = GetMousePosition(new SnapshotPoint(_textView.TextSnapshot, e.Position));
            if (point != null)
            {
                ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position,
                    PointTrackingMode.Positive);
                if (!_componentContext.QuickInfoBroker.IsQuickInfoActive(_textView))
                {
                    _session = _componentContext.QuickInfoBroker.CreateQuickInfoSession(_textView, triggerPoint, true);
                    _session.Start();
                }
            }
        }

        private SnapshotPoint? GetMousePosition(SnapshotPoint topPosition)
        {
            return _textView.BufferGraph.MapDownToFirstMatch(topPosition, PointTrackingMode.Positive,
                snapshot => _subjectBuffers.Contains(snapshot.TextBuffer), PositionAffinity.Predecessor);
        }

        public void Detach(ITextView textView)
        {
            if (_textView.Equals(textView))
            {
                _textView.MouseHover -= OnTextViewMouseHover;
                _textView = null;
            }
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }
    }
}