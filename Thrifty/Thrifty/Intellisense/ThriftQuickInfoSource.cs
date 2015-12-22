using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace Thrifty.Intellisense
{
    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType(Constants.ClassificationName)]
    [Name(Constants.QuickInfoName)]
    class ThriftQuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        [Import]
        private IBufferTagAggregatorFactoryService _bufferTagAggregatorFactoryService;

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new ThriftQuickInfoSource(textBuffer,
                _bufferTagAggregatorFactoryService.CreateTagAggregator<ThriftTokenTag>(textBuffer));
        }
    }

    class ThriftQuickInfoSource : IQuickInfoSource
    {
        private readonly ITagAggregator<ThriftTokenTag> _tagAggregator;
        private readonly ITextBuffer _textBuffer;
        private bool _disposed;
         
        public ThriftQuickInfoSource(ITextBuffer textBuffer, ITagAggregator<ThriftTokenTag> tagAggregator)
        {
            _textBuffer = textBuffer;
            _tagAggregator = tagAggregator;
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
        {
            applicableToSpan = null;

            if (_disposed)
                throw new ObjectDisposedException("ThriftQuickInfoSource");

            SnapshotPoint? snapshotPoint = session.GetTriggerPoint(_textBuffer.CurrentSnapshot);
            if (snapshotPoint == null)
                return;
            SnapshotPoint triggerPoint = (SnapshotPoint) snapshotPoint;

            foreach (
                SnapshotSpan tagSpan in
                    _tagAggregator.GetTags(new SnapshotSpan(triggerPoint, triggerPoint))
                        .Where(currentTag => currentTag.Tag.Type == ThriftTokenTypes.ThriftUnion)
                        .Select(currentTag => currentTag.Span.GetSpans(_textBuffer).First()))
            {
                applicableToSpan = _textBuffer.CurrentSnapshot.CreateTrackingSpan(tagSpan,
                    SpanTrackingMode.EdgeExclusive);
                quickInfoContent.Add("Union-ed!");
            }
        }
    }
}
