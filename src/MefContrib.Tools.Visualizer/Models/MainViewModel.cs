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
    using System.Windows.Input;
    using MefContrib.Tools.Visualizer.Services;
    using Microsoft.ComponentModel.Composition.Diagnostics;

	[Export(typeof(MainViewModel))]
	public sealed class MainViewModel : NotifyObject
	{
		private IPartService fileService;

		private string output;
		private ICommand openFilesCommand;

		private CompositionInfo compositionInfo;

		[ImportingConstructor]
		public MainViewModel(IPartService fileService)
		{
			this.fileService = fileService;

            this.PartDefinitions = new CollectionViewSource();
            this.PartDefinitions.SortDescriptions.Add(new SortDescription("IsRejected", ListSortDirection.Descending));
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
                RaisePropertyChanged("PartDefinitions");
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

		private void LoadParts(IEnumerable<ComposablePartCatalog> catalogs)
        {
            if (catalogs != null && catalogs.Count() > 0)
            {
                var aggregateCatalog = new AggregateCatalog();

                if (catalogs != null)
                {
                    foreach (var catalog in catalogs)
                    {
                        aggregateCatalog.Catalogs.Add(catalog);
                    }
                }

                var container = new CompositionContainer(aggregateCatalog);
                this.compositionInfo = new CompositionInfo(aggregateCatalog, container);

                this.RefreshOutput();
            }
		}

		private void RefreshOutput()
		{
			if (this.compositionInfo != null)
			{
				StringBuilder builder = new StringBuilder();

                var definitions = from x in this.compositionInfo.PartDefinitions
                                  select new PartInfo(x);

                this.partDefinitions.Source = definitions;
                this.partDefinitions.View.MoveCurrentToFirst();
			}
		}

		private enum PartSelection : int
		{
			None = 0,
			All = 1,
			AllRejectedParts = 2,
			RejectedRootCauses = 3
		}
	}
}