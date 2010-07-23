namespace MefContrib.Tools.Visualizer.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition.Primitives;
	using System.Linq;
	using System.Windows.Input;
	using MefContrib.Tools.Visualizer.Commands;
	using Microsoft.ComponentModel.Composition.Diagnostics;

    public class ImportInfo : NotifyObject
    {
        private ImportDefinitionInfo _importDefinitionInfo;
		private Action<UnsuitableExportInfo> _selectAction;

        public ImportInfo(ImportDefinitionInfo importDefinitionInfo, Action<UnsuitableExportInfo> selectAction)
        {
            this._importDefinitionInfo = importDefinitionInfo;
			this._selectAction = selectAction;

			this.SelectCommand = new RelayCommand(param => { _selectAction(param as UnsuitableExportInfo); });
        }

		public ICommand SelectCommand
		{
			get;
			private set;
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

		public Exception ActualException
		{
			get { return this._importDefinitionInfo.Exception; }
		}

		public ImportCardinality ImportCardinality
		{
			get { return this._importDefinitionInfo.ImportDefinition.Cardinality; }
		}

		public bool HasUnsuitableExportDefinitions
		{
			get
			{
				return this.UnsuitableExportDefinitions.Any();
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

		public bool HasMatchedExportDefinitions
		{
			get
			{
				return this.MatchedExportDefinitions.Any();
			}
		}

		public IEnumerable<ExportInfo> MatchedExportDefinitions
		{
			get
			{
				return this._importDefinitionInfo.Actuals.Select(e => new ExportInfo(e));
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

		public PartDefinitionInfo PartDefinition
		{
			get
			{
				return this._unsuitableExportDefinitionInfo.PartDefinition;
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
