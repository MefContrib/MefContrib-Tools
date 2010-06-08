namespace MefContrib.Tools.Visualizer.Services
{
	using System.ComponentModel.Composition;

	[Export(typeof(IPartService))]
	public partial class PartService : IPartService
	{
	}
}