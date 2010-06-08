namespace MefContrib.Tools.Visualizer.Models
{
	using System.ComponentModel.Composition;

    [Export(typeof(ShellViewModel))]
	public sealed class ShellViewModel : NotifyObject
	{
	}
}