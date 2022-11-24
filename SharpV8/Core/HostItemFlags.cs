// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Dynamic;

namespace Microsoft.ClearScript
{
    /// <summary>
    /// Defines options for exposing host resources to script code.
    /// </summary>
    [Flags]
    public enum HostItemFlags
    {
        /// <summary>
        /// Specifies that no options are selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the host resource's members are to be exposed as global items in the
        /// script engine's root namespace.
        /// </summary>
        GlobalMembers = 0x00000001,

        /// <summary>
        /// Specifies that the host resource's non-public members are to be exposed.
        /// </summary>
        PrivateAccess = 0x00000002,

        /// <summary>
        /// Specifies that the host resource's dynamic members are not to be exposed. This option
        /// applies only to objects that implement <c><see cref="IDynamicMetaObjectProvider"/></c>.
        /// </summary>
        HideDynamicMembers = 0x00000004,
    }
}
