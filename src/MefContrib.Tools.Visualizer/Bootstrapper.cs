namespace MefContrib.Tools.Visualizer
{
	internal sealed class Bootstrapper
	{
		public void Run()
		{
			this.CreateShell();
		}

		private void CreateShell()
		{
			Shell shell = new Shell();
			shell.Show();
		}
	}
}