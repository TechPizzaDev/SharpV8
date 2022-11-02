// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using SharpV8.ICUData;

namespace SharpV8.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            byte[] icuBytes = V8ICUData.GetBytes();
            V8Proxy.InitializeICU(icuBytes);

            var switcher = new BenchmarkSwitcher(new[] { typeof(CallMethodWithVector) });

            switcher.RunAllJoined(new Config());
        }
    }

    public class Config : ManualConfig
    {
        public Config()
        {
            AddLogger(new ConsoleLogger());
            AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByParams);
            AddExporter(new HtmlExporter());
            AddColumnProvider(DefaultColumnProviders.Instance);

            AddJob(Job.ShortRun
                .WithLaunchCount(1)
                .WithEvaluateOverhead(true)
                .WithToolchain(InProcessEmitToolchain.Instance));
        }
    }
}
