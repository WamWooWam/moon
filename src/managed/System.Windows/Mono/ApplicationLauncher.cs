//
// ApplicationLauncher.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2007 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

using Mono.Xaml;

namespace Mono
{

    /// <summary>
    ///   A class that provides basic services to launch the managed Silverlight code
    ///   Used by the plugin as an entry point.
    /// </summary>	
    static class ApplicationLauncher
    {
        static bool deployment_initialized;

        /// <summary>
        ///   Ensures that a managed peer has been created for a given native dependencyobject.
        /// </summary>
        public static void EnsureManagedPeer(IntPtr forDO, Kind kind)
        {
            try
            {
                if (!deployment_initialized)
                {
                    deployment_initialized = true;
                    if (forDO == Deployment.Current.native)
                        return;
                }

                var o = NativeDependencyObjectHelper.CreateObject(kind, forDO);
#if DEBUG_REF
				 Console.WriteLine ("Creating managed peer {0}/{1} for {2:X}", o.GetHashCode(), o.GetType(), forDO);
#endif
                GC.KeepAlive(o);
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine("Moonlight: Unhandled exception in ApplicationLauncher.EnsureManagedPeer: {0}", ex);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///   Creates a new Loader for a XAML file.
        /// </summary>
        public static ManagedXamlLoader CreateXamlLoader(IntPtr native_loader, IntPtr plugin, IntPtr surface, IntPtr resourceBase)
        {
            var loader = XamlLoader.CreateManagedXamlLoader(UriHelper.FromGCHandle(resourceBase), surface, plugin) as ManagedXamlLoader;
            loader.Setup(native_loader, plugin, surface);
            return loader;
        }

        /// <summary>
        ///   Loads the Application contained in the XAP file into the current AppDomain
        /// </summary>
        public static bool InitializeDeployment(IntPtr plugin, string xapFile, string culture, string uiCulture)
        {
            return Deployment.Current.InitializeDeployment(plugin, xapFile, culture, uiCulture);
        }

        public static bool InitializeDeployment(string culture, string uiCulture)
        {
            return Deployment.Current.InitializeDeployment(culture, uiCulture);
        }

        /// <summary>
        ///   Shutsdown the Application contained in the XAP file in the current AppDomain
        /// </summary>
        public static void DestroyApplication(IntPtr plugin)
        {
            if (Application.Current != null)
                Application.Current.Terminate();
        }
    }
}

