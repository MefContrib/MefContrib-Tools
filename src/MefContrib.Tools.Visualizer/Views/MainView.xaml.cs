namespace MefContrib.Tools.Visualizer.Views
{
	using System.ComponentModel.Composition;
	using System.Windows.Controls;
	using MefContrib.Tools.Visualizer.Models;

	public partial class MainView : UserControl
	{
		public MainView()
		{
			InitializeComponent();

            CompositionInitializer.SatisfyImports(this);
		}

		[Import]
		public MainViewModel ViewModel
		{
			set
			{
				this.DataContext = value;
			}
		}
	}
}