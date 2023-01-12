// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Numerics;
using System.Threading;
using BenchmarkDotNet.Attributes;
using Microsoft.ClearScript.V8;

namespace SharpV8.Benchmarks
{
    public class CallMethodWithVector : V8ScriptEngineBench
    {
        [Params(/*1,*/ 1000)]
        public object Count { get; set; } = new();

        public override void Setup()
        {
            base.Setup();
        }

        [Benchmark]
        public void Vec1()
        {
            Engine.Invoke("runVec1", Count, 1.234);
        }

        [Benchmark]
        public void Vec2()
        {
            Engine.Invoke("runVec2", Count);
        }

        [Benchmark]
        public void Vec3()
        {
            Engine.Invoke("runVec3", Count);
        }

        [Benchmark]
        public void Vec4()
        {
            Engine.Invoke("runVec4", Count);
        }
    }
}
