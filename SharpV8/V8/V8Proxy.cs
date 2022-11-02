// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.Util;
using Microsoft.ClearScript.V8.SplitProxy;

namespace Microsoft.ClearScript.V8
{
    public abstract partial class V8Proxy : IDisposable
    {
        static V8Proxy()
        {
            V8SplitProxyNative.Imports.ResolveLibrary += LoadNativeLibrary;
        }

        private static IntPtr LoadNativeLibrary(string libraryName, System.Reflection.Assembly assembly, DllImportSearchPath? searchPath)
        {
            V8SplitProxyNative.Imports.GetRuntimeInfo(out string platform, out string architecture, out string extension);
            var fileName = V8SplitProxyNative.Imports.GetDllName(platform, architecture, extension);
            
            var paths = GetDirPaths(platform, architecture).Select(dirPath => Path.Combine(dirPath, fileName)).Distinct();
            foreach (string path in paths)
            {
                if (NativeLibrary.TryLoad(path, assembly, null, out IntPtr hLibrary))
                {
                    return hLibrary;
                }
            }
            return IntPtr.Zero;
        }

        public static unsafe void InitializeICU(ReadOnlySpan<byte> bytes)
        {
            uint length = Convert.ToUInt32(bytes.Length);
            fixed (byte* pBytes = bytes)
            {
                var pICUData = (IntPtr)pBytes;
                V8SplitProxyNative.InvokeNoThrow(() => V8SplitProxyNative.V8Environment_InitializeICU(pICUData, length));
            }
        }

        private static IEnumerable<string> GetDirPaths(string platform, string architecture)
        {
            // The assembly location may be empty if the host preloaded the assembly
            // from custom storage. Support for this scenario was requested on CodePlex.

            var location = typeof(V8Proxy).Assembly.Location;
            if (!string.IsNullOrWhiteSpace(location))
            {
                if ((platform != null) && (architecture != null))
                {
                    yield return Path.Combine(Path.GetDirectoryName(location), "runtimes", $"{platform}-{architecture}", "native");
                }

                yield return Path.GetDirectoryName(location);
            }

            var appDomain = AppDomain.CurrentDomain;
            yield return appDomain.BaseDirectory;

            var searchPath = appDomain.RelativeSearchPath;
            if (!string.IsNullOrWhiteSpace(searchPath))
            {
                foreach (var dirPath in searchPath.SplitSearchPath())
                {
                    yield return dirPath;
                }
            }

            searchPath = HostSettings.AuxiliarySearchPath;
            if (!string.IsNullOrWhiteSpace(searchPath))
            {
                foreach (var dirPath in searchPath.SplitSearchPath())
                {
                    yield return dirPath;
                }
            }
        }

        #region IDisposable implementation (abstract)

        public abstract void Dispose();

        #endregion
    }
}
