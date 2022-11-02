// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using BenchmarkDotNet.Attributes;
using Microsoft.ClearScript.V8;

namespace SharpV8.Benchmarks
{
    public abstract class V8ScriptEngineBench
    {
        public V8ScriptEngineFlags EngineFlags { get; set; } = V8ScriptEngineFlags.None;

        public V8ScriptEngine Engine { get; private set; }

        [GlobalSetup]
        public virtual void Setup()
        {
            Engine = new V8ScriptEngine(EngineFlags);
        }

        [GlobalCleanup]
        public virtual void Cleanup()
        {
            Engine.Dispose();
        }
    }
}
