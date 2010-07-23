namespace MefContrib.Tools.Visualizer.Converters
{
	using System;
	using System.ComponentModel.Composition;
	using System.ComponentModel.Composition.Primitives;
	using System.Linq;
	using System.Windows.Data;
	using System.Windows.Media;
	using MefContrib.Tools.Visualizer.Models;
	using Microsoft.ComponentModel.Composition.Diagnostics;

	public class ImportInfoToColorConverter : IValueConverter
	{
		public Brush CardinalityErrorBrush { get; set; }

		public Brush NoUnsuitableExportsBrush { get; set; }

		public Brush ExportMatchingErrorBrush { get; set; }

		public Brush OptionalExportsUnsuitableBrush { get; set; }

		public Brush ProvidingPartRejectedBrush { get; set; }

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Brush brush = this.NoUnsuitableExportsBrush;

			ImportInfo importInfo = value as ImportInfo;

			if (importInfo != null)
			{
				if (importInfo.UnsuitableExportDefinitions.Any(
						uei => uei.Issues.Any(i => i.Reason != UnsuitableExportDefinitionReason.PartDefinitionIsRejected)))
				{
					brush = this.ExportMatchingErrorBrush;
				}
				else if (!importInfo.HasUnsuitableExportDefinitions)
				{
					if (importInfo.ActualException is ImportCardinalityMismatchException)
					{
						brush = CardinalityErrorBrush;
					}
					else
					{
						brush = this.NoUnsuitableExportsBrush;
					}
				}
				else if (importInfo.ImportCardinality == ImportCardinality.ZeroOrOne ||
					importInfo.ImportCardinality == ImportCardinality.ZeroOrMore)
				{
					brush = this.OptionalExportsUnsuitableBrush;
				}
				else if (true)
				{
					brush = this.ProvidingPartRejectedBrush;
				}

			}

			return brush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
