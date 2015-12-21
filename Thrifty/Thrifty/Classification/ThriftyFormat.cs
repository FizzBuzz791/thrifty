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
    internal sealed class ThriftyFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Thrifty" classification type
        /// </summary>
        public ThriftyFormat()
        {
            DisplayName = Constants.Keywords.Union; //human readable version of the name
            ForegroundColor = Colors.BlueViolet;
        }
    }
}