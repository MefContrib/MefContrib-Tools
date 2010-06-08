// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------

namespace MefContrib.Tools.Visualizer.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Reflection;

    public class CatalogHelper
    {
        public static void DiscoverParts(AggregateCatalog catalog, IEnumerable<Assembly> assemblies)
        {
            using (var atomicComposition = new AtomicComposition())
            {
                foreach (var assembly in assemblies)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("{0}", assembly.FullName));

                    catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                }

                atomicComposition.Complete();
            }
        }
    }
}