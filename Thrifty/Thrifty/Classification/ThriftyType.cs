using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Classification
{
    internal static class ThriftyClassificationDefinition
    {
        [Export(typeof (ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftKeyword = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Union)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftUnion = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Struct)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftStruct = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Namespace)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftNamespace = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Include)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftInclude = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Required)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftRequired = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Optional)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftOptional = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Double)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftDouble = null;

        //[Export(typeof(ClassificationTypeDefinition))]
        //[Name(Constants.Keywords.String)]
        //[BaseDefinition(Constants.Keywords.Keyword)]
        //internal static ClassificationTypeDefinition ThriftString = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Int64)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftInt64 = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Int32)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftInt32 = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(Constants.Keywords.Bool)]
        [BaseDefinition(Constants.Keywords.Keyword)]
        internal static ClassificationTypeDefinition ThriftBool = null;
    }
}