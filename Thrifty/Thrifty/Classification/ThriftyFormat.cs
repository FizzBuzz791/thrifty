using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Thrifty.Classification
{
    /// <summary>
    /// Defines an editor format for the Thrifty type that has a purple foreground.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Union)]
    [Name(Constants.Keywords.Union)]
    [UserVisible(false)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class UnionFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Thrifty" classification type
        /// </summary>
        public UnionFormat()
        {
            DisplayName = Constants.Keywords.Union; //human readable version of the name
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Struct)]
    [Name(Constants.Keywords.Struct)]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class StructFormat : ClassificationFormatDefinition
    {
        public StructFormat()
        {
            DisplayName = Constants.Keywords.Struct;
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Include)]
    [Name(Constants.Keywords.Include)]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class IncludeFormat : ClassificationFormatDefinition
    {
        public IncludeFormat()
        {
            DisplayName = Constants.Keywords.Include;
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Namespace)]
    [Name(Constants.Keywords.Namespace)]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NamespaceFormat : ClassificationFormatDefinition
    {
        public NamespaceFormat()
        {
            DisplayName = Constants.Keywords.Namespace;
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Required)]
    [Name(Constants.Keywords.Required)]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class RequiredFormat : ClassificationFormatDefinition
    {
        public RequiredFormat()
        {
            DisplayName = Constants.Keywords.Required;
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Constants.Keywords.Optional)]
    [Name(Constants.Keywords.Optional)]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class OptionalFormat : ClassificationFormatDefinition
    {
        public OptionalFormat()
        {
            DisplayName = Constants.Keywords.Optional;
            ForegroundColor = Color.FromRgb(86, 156, 214);
        }
    }
}