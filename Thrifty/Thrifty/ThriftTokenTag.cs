using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(Constants.ClassificationName)]
    [TagType(typeof(ThriftTokenTag))]
    internal sealed class ThriftTokenTagProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new ThriftTokenTagger() as ITagger<T>;
        }
    }

    public class ThriftTokenTag : ITag
    {
        public ThriftTokenTypes Type { get; private set; }

        public ThriftTokenTag(ThriftTokenTypes type)
        {
            Type = type;
        }
    }

    internal sealed class ThriftTokenTagger : ITagger<ThriftTokenTag>
    {
        readonly IDictionary<string, ThriftTokenTypes> _thriftTypes;

        internal ThriftTokenTagger()
        {
            _thriftTypes = new Dictionary<string, ThriftTokenTypes>
            {
                [Constants.Keywords.Union] = ThriftTokenTypes.ThriftUnion,
                [Constants.Keywords.Struct] = ThriftTokenTypes.ThriftStruct,
                [Constants.Keywords.Namespace] = ThriftTokenTypes.ThriftNamespace,
                [Constants.Keywords.Include] = ThriftTokenTypes.ThriftInclude,
                [Constants.Keywords.Required] = ThriftTokenTypes.ThriftRequired,
                [Constants.Keywords.Optional] = ThriftTokenTypes.ThriftOptional,
                [Constants.Keywords.Double] = ThriftTokenTypes.ThriftDouble,
                //[Constants.Keywords.String] = ThriftTokenTypes.ThriftString,
                [Constants.Keywords.Int64] = ThriftTokenTypes.ThriftInt64,
                [Constants.Keywords.Int32] = ThriftTokenTypes.ThriftInt32,
                [Constants.Keywords.Bool] = ThriftTokenTypes.ThriftBool
            };
        }
        
        public IEnumerable<ITagSpan<ThriftTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan currentSpan in spans)
            {
                ITextSnapshotLine containingLine = currentSpan.Start.GetContainingLine();
                int currentLocation = containingLine.Start.Position;
                string[] tokens = containingLine.GetText().ToLower().Split(' ');

                foreach (string thriftToken in tokens)
                {
                    if (_thriftTypes.ContainsKey(thriftToken))
                    {
                        SnapshotSpan tokenSpan = new SnapshotSpan(currentSpan.Snapshot,
                            new Span(currentLocation, thriftToken.Length));
                        if (tokenSpan.IntersectsWith(currentSpan))
                            yield return
                                new TagSpan<ThriftTokenTag>(tokenSpan, new ThriftTokenTag(_thriftTypes[thriftToken]));
                    }

                    // add an extra char location because of the space
                    currentLocation += thriftToken.Length + 1;
                }
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }
    }
}