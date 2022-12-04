// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.ClearScript.Util
{
    internal sealed class Nonexistent
    {
        public static Nonexistent Value { get; } = new Nonexistent();

        private Nonexistent()
        {
        }
    }
}
