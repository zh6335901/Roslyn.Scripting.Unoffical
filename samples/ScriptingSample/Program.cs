using Unofficial.CodeAnalysis.CSharp.Scripting;
using Unofficial.CodeAnalysis.Scripting.Hosting;
using Unofficial.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis;
using ScriptingSample.Lib;
using System.Diagnostics;
using ScriptingSample;

while (true)
{
    var loader = new InteractiveAssemblyLoader();
    var options = ScriptOptions.Default
        .AddReferences(MetadataReference.CreateFromFile(typeof(TestClass).Assembly.Location))
        .WithImports("System", "ScriptingSample.Lib");

    var code = $$$"""
        var json = new TestClass().ToJson(1);
        var globalsName = Name;
        """;

    var script = CSharpScript.Create(code, options, globalsType: typeof(Globals), assemblyLoader: loader);
    var result = await script.RunAsync(new Globals()).ConfigureAwait(false);
    result = await result.ContinueWithAsync("1 + 2").ConfigureAwait(false);
    result = await result.ContinueWithAsync("new TestClass().ToJson(\"aaa\")").ConfigureAwait(false);
    result = await result.ContinueWithAsync("new TestClass().ToJson(false)").ConfigureAwait(false);
    result = await result.ContinueWithAsync("1 + 3").ConfigureAwait(false);

    loader.Dispose();

    GC.Collect();
    GC.WaitForPendingFinalizers();

    await Task.Delay(100).ConfigureAwait(false);

    var currentProcess = Process.GetCurrentProcess();
    Console.WriteLine($"Process handle count: {currentProcess.HandleCount}");
}