namespace MefContrib.Tools.Visualizer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.ComponentModel.Composition.Diagnostics;

    public class ImportInfo : NotifyObject
    {
        private ImportDefinitionInfo _importDefinitionInfo;

        public ImportInfo(ImportDefinitionInfo importDefinitionInfo)
        {
            this._importDefinitionInfo = importDefinitionInfo;
        }

        public string DisplayName
        {
            get
            {
                return CompositionElementTextFormatter.DisplayCompositionElement(this._importDefinitionInfo.ImportDefinition);
            }
        }

        public string Exception
        {
            get
            {
                string exception = this._importDefinitionInfo.Exception != null ?
                    this._importDefinitionInfo.Exception.ToString() : String.Empty;

                return exception;
            }
        }

        public IEnumerable<UnsuitableExportInfo> UnsuitableExportDefinitions
        {
            get
            {
                var results = from x in this._importDefinitionInfo.UnsuitableExportDefinitions
                              select new UnsuitableExportInfo(x);

                return results;
            }
        }
    }

    public class UnsuitableExportInfo
    {
        private UnsuitableExportDefinitionInfo _unsuitableExportDefinitionInfo;

        public UnsuitableExportInfo(UnsuitableExportDefinitionInfo unsuitableExportDefinitionInfo)
        {
            this._unsuitableExportDefinitionInfo = unsuitableExportDefinitionInfo;
        }

        public string DisplayName
        {
            get
            {
                return CompositionElementTextFormatter.DescribeCompositionElement(this._unsuitableExportDefinitionInfo.ExportDefinition);
            }
        }

        public IEnumerable<UnsuitableExportDefinitionIssue> Issues
        {
            get
            {
                return this._unsuitableExportDefinitionInfo.Issues;
            }
        }
    }
}
