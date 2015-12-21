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

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Struct)]
        internal static ClassificationTypeDefinition ThriftStruct = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Namespace)]
        internal static ClassificationTypeDefinition ThriftNamespace = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Include)]
        internal static ClassificationTypeDefinition ThriftInclude = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Required)]
        internal static ClassificationTypeDefinition ThriftRequired = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Optional)]
        internal static ClassificationTypeDefinition ThriftOptional = null;
    }
}