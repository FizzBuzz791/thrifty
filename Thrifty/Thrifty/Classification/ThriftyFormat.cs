using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Classification
{
    /// <summary>
    /// This is the base format that all Constant.Keywords.* inherit from.
    /// </summary>
    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Keyword)]
    [Name(Constants.Keywords.Keyword)]
    [DisplayName(Constants.Keywords.Keyword)]
    [UserVisible(true)] // This causes the definition to show up in "Fonts and Colors".
    internal sealed class KeywordFormat : ClassificationFormatDefinition
    {
        public KeywordFormat()
        {
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Union)]
    [Name(Constants.Keywords.Union)]
    internal sealed class UnionFormat : ClassificationFormatDefinition
    {
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Struct)]
    [Name(Constants.Keywords.Struct)]
    internal sealed class StructFormat : ClassificationFormatDefinition
    {
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Include)]
    [Name(Constants.Keywords.Include)]
    internal sealed class IncludeFormat : ClassificationFormatDefinition
    {
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Namespace)]
    [Name(Constants.Keywords.Namespace)]
    internal sealed class NamespaceFormat : ClassificationFormatDefinition
    {
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Required)]
    [Name(Constants.Keywords.Required)]
    internal sealed class RequiredFormat : ClassificationFormatDefinition
    {
    }

    [Export(typeof (EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Optional)]
    [Name(Constants.Keywords.Optional)]
    internal sealed class OptionalFormat : ClassificationFormatDefinition
    {
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Double)]
    [Name(Constants.Keywords.Double)]
    internal sealed class DoubleFormat : ClassificationFormatDefinition
    {
    }

    //[Export(typeof(EditorFormatDefinition))]
    //[ClassificationType(ClassificationTypeNames = Constants.Keywords.String)]
    //[Name(Constants.Keywords.String)]
    //internal sealed class StringFormat : ClassificationFormatDefinition
    //{
    //}

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Int64)]
    [Name(Constants.Keywords.Int64)]
    internal sealed class Int64Format : ClassificationFormatDefinition
    {
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Int32)]
    [Name(Constants.Keywords.Int32)]
    internal sealed class Int32Format : ClassificationFormatDefinition
    {
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Bool)]
    [Name(Constants.Keywords.Bool)]
    internal sealed class BoolFormat : ClassificationFormatDefinition
    {
    }
}