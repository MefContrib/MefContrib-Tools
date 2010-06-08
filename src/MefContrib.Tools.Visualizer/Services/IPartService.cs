namespace MefContrib.Tools.Visualizer.Services
{
	using System;
	using System.Collections.Generic;
    using System.ComponentModel.Composition.Primitives;

	public interface IPartService
	{
        void PromptForParts(Action<IEnumerable<ComposablePartCatalog>> callback);
	}
}