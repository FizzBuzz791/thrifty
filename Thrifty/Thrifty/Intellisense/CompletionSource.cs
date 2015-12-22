using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Intellisense
{
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType(Constants.ClassificationName)]
    [Name(Constants.CompletionName)]
    class ThriftCompletionSourceProvider : ICompletionSourceProvider
    {
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new ThriftCompletionSource(textBuffer);
        }
    }

    class ThriftCompletionSource : ICompletionSource
    {
        private readonly ITextBuffer _textBuffer;
        private bool _disposed;

        public ThriftCompletionSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_disposed)
                throw new ObjectDisposedException("ThriftCompletionSource");

            List<Completion> completions = new List<Completion>
            {
                new Completion(Constants.Keywords.Union),
                new Completion(Constants.Keywords.Struct),
                new Completion(Constants.Keywords.Namespace),
                new Completion(Constants.Keywords.Include),
                new Completion(Constants.Keywords.Required),
                new Completion(Constants.Keywords.Optional)
            };

            ITextSnapshot snapshot = _textBuffer.CurrentSnapshot;
            SnapshotPoint? snapshotPoint = session.GetTriggerPoint(snapshot);
            if (snapshotPoint == null)
                return;

            SnapshotPoint triggerPoint = (SnapshotPoint)snapshotPoint;

            ITextSnapshotLine line = triggerPoint.GetContainingLine();
            SnapshotPoint start = triggerPoint;

            while (start > line.Start && !char.IsWhiteSpace((start - 1).GetChar()))
                start -= 1;

            ITrackingSpan applicableTo = snapshot.CreateTrackingSpan(new SnapshotSpan(start, triggerPoint),
                SpanTrackingMode.EdgeInclusive);
            completionSets.Add(new CompletionSet("All", "All", applicableTo, completions, Enumerable.Empty<Completion>()));
        }
    }
}