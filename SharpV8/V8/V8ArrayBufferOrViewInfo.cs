// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.ClearScript.V8
{
    public readonly struct V8ArrayBufferOrViewInfo
    {
        public V8ArrayBufferOrViewInfo(V8ArrayBufferOrViewKind kind, V8Object arrayBuffer, ulong offset, ulong size, ulong length)
        {
            Kind = kind;
            ArrayBuffer = arrayBuffer;
            Offset = offset;
            Size = size;
            Length = length;
        }

        public V8ArrayBufferOrViewKind Kind { get; }

        public V8Object ArrayBuffer { get; }

        public ulong Offset { get; }

        public ulong Size { get; }

        public ulong Length { get; }
    }
}
