using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Classification
{
    internal static class ThriftyClassificationDefinition
    {
        /// <summary>
        /// Defines the "Thrifty" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Union)]
        internal static ClassificationTypeDefinition ThriftUnion = null;
    }
}