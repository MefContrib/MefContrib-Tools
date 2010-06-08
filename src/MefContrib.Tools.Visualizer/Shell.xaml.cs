
namespace MefContrib.Tools.Visualizer
{
	using System.Windows;
	using System.ComponentModel.Composition;
	using MefContrib.Tools.Visualizer.Models;

	public partial class Shell : Window
	{
		public Shell()
		{
			InitializeComponent();

			CompositionInitializer.SatisfyImports(this);
		}

		[Import]
		public ShellViewModel ViewModel
		{
			set
			{
				this.DataContext = value;
			}
		}
	}
}