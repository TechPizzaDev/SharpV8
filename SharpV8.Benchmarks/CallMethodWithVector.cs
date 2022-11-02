// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Numerics;
using BenchmarkDotNet.Attributes;
using Microsoft.ClearScript.V8;

namespace SharpV8.Benchmarks
{
    public class CallMethodWithVector : V8ScriptEngineBench
    {
        [Params(1, 1000)]
        public object Count { get; set; }

        public override void Setup()
        {
            base.Setup();

            Engine.AddHostObject("entity", new Entity());
            Engine.AddHostType(typeof(Vector2));
            Engine.AddHostType(typeof(Vector3));
            Engine.AddHostType(typeof(Vector4));

            Engine.Execute(
                @"function runVec2(count) { let v = new Vector2(1, 2);       for(let i = 0; i < count; i++) entity.SetPosition(v); }" +
                @"function runVec3(count) { let v = new Vector3(1, 2, 3);    for(let i = 0; i < count; i++) entity.SetPosition(v); }" +
                @"function runVec4(count) { let v = new Vector4(1, 2, 3, 4); for(let i = 0; i < count; i++) entity.SetPosition(v); }");
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

        public class Entity
        {
            public void SetPosition(Vector2 vec)
            {
            }

            public void SetPosition(Vector3 vec)
            {
            }

            public void SetPosition(Vector4 vec)
            {
            }
        }
    }
}
