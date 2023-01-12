// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Numerics;
using BenchmarkDotNet.Attributes;
using Microsoft.ClearScript.V8;

namespace SharpV8.Benchmarks
{
    public abstract class V8ScriptEngineBench
    {
        public static V8Runtime Runtime { get; } = new("BenchmarkRuntime");

        public static V8ScriptEngineFlags EngineFlags { get; set; } = V8ScriptEngineFlags.None;

        public static V8ScriptEngine Engine { get; private set; }

        static V8ScriptEngineBench()
        {
            Engine = Runtime.CreateScriptEngine("BenchmarkEngine", EngineFlags);
            
            Script1 = Runtime.Compile("s1.js", @"function runVec1(count, v) { for(let i = 0; i < count; i++) entity.SetPosition(v); }");
            Script2 = Runtime.Compile("s2.js", @"function runVec2(count) { let v = new Vector2(1, 2); for(let i = 0; i < count; i++) entity.SetPosition(v); }");
            Script3 = Runtime.Compile("s3.js", @"function runVec3(count) { let v = new Vector3(1, 2, 3); for(let i = 0; i < count; i++) entity.SetPosition(v); }");
            Script4 = Runtime.Compile("s4.js", @"function runVec4(count) { let v = new Vector4(1, 2, 3, 4); for(let i = 0; i < count; i++) entity.SetPosition(v); }");

            Engine.AddHostObject("entity", new Entity());
            Engine.AddHostType(typeof(Vector2));
            Engine.AddHostType(typeof(Vector3));
            Engine.AddHostType(typeof(Vector4));

            Engine.Execute(Script1);
            Engine.Execute(Script2);
            Engine.Execute(Script3);
            Engine.Execute(Script4);
        }

        [GlobalSetup]
        public virtual void Setup()
        {
        }

        [GlobalCleanup]
        public virtual void Cleanup()
        {
            //Engine.Dispose();
        }

        public static V8Script Script1;
        public static V8Script Script2;
        public static V8Script Script3;
        public static V8Script Script4;

        public class Entity
        {
            public void SetPosition(double vec)
            {
            }

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
