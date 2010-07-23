namespace MefContrib.Tools.Visualizer.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
	using MefContrib.Tools.Visualizer.Services;
    using Microsoft.ComponentModel.Composition.Diagnostics;

	[Export(typeof(MainViewModel))]
	public sealed class MainViewModel : NotifyObject
	{
		private IPartService fileService;
		private CompositionContainer _container;
		private CompositionInfo _compositionInfo;
		private AggregateCatalog _aggregateCatalog;
		private Dictionary<PartDefinitionInfo, PartInfoViewModel> _partViewModelMap;

		[ImportingConstructor]
		public MainViewModel(IPartService fileService)
		{
			this.fileService = fileService;

			_aggregateCatalog = new AggregateCatalog();

			_partViewModelMap = new Dictionary<PartDefinitionInfo, PartInfoViewModel>();

            this.PartDefinitions = new CollectionViewSource();
			this.PartDefinitions.SortDescriptions.Add(new SortDescription("IsRejected", ListSortDirection.Descending));
		}

		private PartInfoViewModel _selectedPart;
		public PartInfoViewModel SelectedPart
		{
			get { return _selectedPart; }
			set
			{
				_selectedPart = value;
				RaisePropertyChanged(() => SelectedPart);
			}
		}

        private CollectionViewSource partDefinitions;
        public CollectionViewSource PartDefinitions
        {
            get
            {
                return partDefinitions;
            }
            set
            {
                partDefinitions = value;
				RaisePropertyChanged(() => PartDefinitions);
            }
        }

        public void OpenFiles()
        {
            this.fileService.PromptForParts(
                result =>
                {
                    this.LoadParts(result);
                });
        }

		public void Reset()
		{
			_aggregateCatalog = new AggregateCatalog();

			this.RefreshOutput();
		}

		public void SetSelectedPart(PartDefinitionInfo partDefinition)
		{
			this.SelectedPart = this._partViewModelMap[partDefinition];
		}

		private void ResetContainer()
		{
			//  Dispose any previous container, so that modifying aggregate catalog
			//  won't cause recomposition failures
			if (_container != null)
			{
				_container.Dispose();
				_container = null;
			}
		}

		private void LoadParts(IEnumerable<ComposablePartCatalog> catalogs)
        {
			if (catalogs != null && catalogs.Any())
			{
				ResetContainer();

				if (catalogs != null)
				{
					foreach (var catalog in catalogs)
					{
						_aggregateCatalog.Catalogs.Add(catalog);
					}
				}

				this.RefreshOutput();
			}
		}

		private void RefreshOutput()
		{
			ResetContainer();

			this._container = new CompositionContainer(_aggregateCatalog);
			{
				this._compositionInfo = new CompositionInfo(_aggregateCatalog, _container);

				if (this._compositionInfo != null)
				{
					_partViewModelMap = this._compositionInfo.PartDefinitions.ToDictionary(pd => pd, pd => new PartInfoViewModel(pd, this));

					this.partDefinitions.Source = _partViewModelMap.Values.ToList();
					this.partDefinitions.View.MoveCurrentToFirst();
				}
			}
		}
	}
}