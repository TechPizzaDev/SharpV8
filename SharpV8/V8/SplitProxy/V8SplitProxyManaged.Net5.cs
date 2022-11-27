// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.ClearScript.Util;

namespace Microsoft.ClearScript.V8.SplitProxy
{
    internal static partial class V8SplitProxyManaged
    {
        #region fast method pointers

        private static unsafe IntPtr CacheV8ObjectFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pCache, IntPtr pObject, IntPtr pV8Object)
                {
                    CacheV8Object(pCache, pObject, pV8Object);
                }

                delegate* unmanaged[Stdcall]<IntPtr, IntPtr, IntPtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr GetCachedV8ObjectFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static IntPtr Thunk(IntPtr pCache, IntPtr pObject)
                {
                    return GetCachedV8Object(pCache, pObject);
                }

                delegate* unmanaged[Stdcall]<IntPtr, IntPtr, IntPtr> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr AddRefHostObjectFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static IntPtr Thunk(IntPtr pObject)
                {
                    return AddRefHostObject(pObject);
                }

                delegate* unmanaged[Stdcall]<IntPtr, IntPtr> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr ReleaseHostObjectFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject)
                {
                    ReleaseHostObject(pObject);
                }

                delegate* unmanaged[Stdcall]<IntPtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr GetHostObjectInvocabilityFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static Invocability Thunk(IntPtr pObject)
                {
                    return GetHostObjectInvocability(pObject);
                }

                delegate* unmanaged[Stdcall]<IntPtr, Invocability> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr InvokeHostActionFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pAction)
                {
                    InvokeHostAction(pAction);
                }

                delegate* unmanaged[Stdcall]<IntPtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr GetHostObjectNamedPropertyFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, StdStringPtr pName, V8ValuePtr pValue)
                {
                    GetHostObjectNamedProperty(pObject, pName, pValue);
                }

                delegate* unmanaged[Stdcall]<IntPtr, StdStringPtr, V8ValuePtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr GetHostObjectNamedPropertyWithCacheabilityFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, StdStringPtr pName, V8ValuePtr pValue, sbyte* pIsCacheable)
                {
                    GetHostObjectNamedPropertyWithCacheability(pObject, pName, pValue, out bool isCacheable);
                    *pIsCacheable = isCacheable.ToSbyte();
                }

                delegate* unmanaged[Stdcall]<IntPtr, StdStringPtr, V8ValuePtr, sbyte*, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr SetHostObjectNamedPropertyFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, StdStringPtr pName, V8ValuePtr pValue)
                {
                    SetHostObjectNamedProperty(pObject, pName, pValue);
                }

                delegate* unmanaged[Stdcall]<IntPtr, StdStringPtr, V8ValuePtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr GetHostObjectIndexedPropertyFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, int index, V8ValuePtr pValue)
                {
                    GetHostObjectIndexedProperty(pObject, index, pValue);
                }

                delegate* unmanaged[Stdcall]<IntPtr, int, V8ValuePtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr SetHostObjectIndexedPropertyFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, int index, V8ValuePtr pValue)
                {
                    SetHostObjectIndexedProperty(pObject, index, pValue);
                }

                delegate* unmanaged[Stdcall]<IntPtr, int, V8ValuePtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr InvokeHostObjectFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, sbyte asConstructor, StdV8ValueArrayPtr pArgs, V8ValuePtr pResult)
                {
                    InvokeHostObject(pObject, asConstructor.ToBool(), pArgs, pResult);
                }

                delegate* unmanaged[Stdcall]<IntPtr, sbyte, StdV8ValueArrayPtr, V8ValuePtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        private static unsafe IntPtr InvokeHostObjectMethodFastMethodPtr
        {
            get
            {
                [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
                static void Thunk(IntPtr pObject, StdStringPtr pName, StdV8ValueArrayPtr pArgs, V8ValuePtr pResult)
                {
                    InvokeHostObjectMethod(pObject, pName, pArgs, pResult);
                }

                delegate* unmanaged[Stdcall]<IntPtr, StdStringPtr, StdV8ValueArrayPtr, V8ValuePtr, void> pThunk = &Thunk;
                return (IntPtr)pThunk;
            }
        }

        #endregion
    }
}
