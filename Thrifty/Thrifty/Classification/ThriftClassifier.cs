using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Classification
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.ClassificationName)]
    [TagType(typeof(ClassificationTag))]
    internal sealed class ThriftClassifierProvider : ITaggerProvider
    {
        [Export]
        [Name(Constants.ClassificationName)]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition ThriftContentType = null;

        [Export]
        [FileExtension(".thrift")]
        [ContentType(Constants.ClassificationName)]
        internal static FileExtensionToContentTypeDefinition ThriftFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry;

        [Import]
        internal IBufferTagAggregatorFactoryService AggregatorFactory;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            ITagAggregator<ThriftTokenTag> thriftTagAggregator =
                AggregatorFactory.CreateTagAggregator<ThriftTokenTag>(buffer);
            return new ThriftClassifier(buffer, thriftTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class ThriftClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer;
        ITagAggregator<ThriftTokenTag> _thriftTagAggregator;
        IDictionary<ThriftTokenTypes, IClassificationType> _thriftTypes;

        internal ThriftClassifier(ITextBuffer buffer, ITagAggregator<ThriftTokenTag> thriftTagAggregator,
            IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _thriftTagAggregator = thriftTagAggregator;
            _thriftTypes = new Dictionary<ThriftTokenTypes, IClassificationType>();
            _thriftTypes[ThriftTokenTypes.ThriftUnion] = typeService.GetClassificationType(Constants.Keywords.Union);
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (IMappingTagSpan<ThriftTokenTag> tagSpan in _thriftTagAggregator.GetTags(spans))
            {
                NormalizedSnapshotSpanCollection tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return
                    new TagSpan<ClassificationTag>(tagSpans[0],
                                                   new ClassificationTag(_thriftTypes[tagSpan.Tag.Type]));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add{}
            remove{}
        }
    }
}