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
        [BaseDefinition("text")]
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
            return new ThriftyClassifier(thriftTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class ThriftyClassifier : ITagger<ClassificationTag>
    {
        readonly ITagAggregator<ThriftTokenTag> _thriftTagAggregator;
        readonly IDictionary<ThriftTokenTypes, IClassificationType> _thriftTypes;

        internal ThriftyClassifier(ITagAggregator<ThriftTokenTag> thriftTagAggregator,
            IClassificationTypeRegistryService typeService)
        {
            _thriftTagAggregator = thriftTagAggregator;
            _thriftTypes = new Dictionary<ThriftTokenTypes, IClassificationType>
            {
                [ThriftTokenTypes.ThriftUnion] = typeService.GetClassificationType(Constants.Keywords.Union),
                [ThriftTokenTypes.ThriftStruct] = typeService.GetClassificationType(Constants.Keywords.Struct),
                [ThriftTokenTypes.ThriftNamespace] = typeService.GetClassificationType(Constants.Keywords.Namespace),
                [ThriftTokenTypes.ThriftInclude] = typeService.GetClassificationType(Constants.Keywords.Include),
                [ThriftTokenTypes.ThriftRequired] = typeService.GetClassificationType(Constants.Keywords.Required),
                [ThriftTokenTypes.ThriftOptional] = typeService.GetClassificationType(Constants.Keywords.Optional),
                [ThriftTokenTypes.ThriftDouble] = typeService.GetClassificationType(Constants.Keywords.Double),
                //[ThriftTokenTypes.ThriftString] = typeService.GetClassificationType(Constants.Keywords.String),
                [ThriftTokenTypes.ThriftInt64] = typeService.GetClassificationType(Constants.Keywords.Int64),
                [ThriftTokenTypes.ThriftInt32] = typeService.GetClassificationType(Constants.Keywords.Int32),
                [ThriftTokenTypes.ThriftBool] = typeService.GetClassificationType(Constants.Keywords.Bool)
            };
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return
                _thriftTagAggregator.GetTags(spans)
                    .Select(
                        tagSpan =>
                            new TagSpan<ClassificationTag>(tagSpan.Span.GetSpans(spans[0].Snapshot)[0],
                                new ClassificationTag(_thriftTypes[tagSpan.Tag.Type])));
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add{}
            remove{}
        }
    }
}