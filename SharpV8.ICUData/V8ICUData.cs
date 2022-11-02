// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics;
using System.IO;

namespace SharpV8.ICUData
{
    public static class V8ICUData
    {
        public const string ResourceName = nameof(SharpV8) + "." + nameof(ICUData) + ".icudtl.dat";

        public static Stream GetStream()
        {
            return typeof(V8ICUData).Assembly.GetManifestResourceStream(ResourceName);
        }

        public static byte[] GetBytes()
        {
            using (Stream stream = GetStream())
            {
                var bytes = new byte[stream.Length];

                var length = stream.Read(bytes, 0, bytes.Length);
                Debug.Assert(length == bytes.Length);

                return bytes;
            }
        }
    }
}
