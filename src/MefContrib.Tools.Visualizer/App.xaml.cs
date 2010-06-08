﻿namespace MefContrib.Tools.Visualizer
{
	using System.Windows;

	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Bootstrapper bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}
	}
}