namespace Engine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Core;

public class SkillEntry
{
    public Action<BattleContext> Action { get; init; }
    public int Priority { get; init; }
    public TriggerType Trigger { get; init; }
    public string Source { get; init; }
}

public class SkillEngine
{
    private readonly Dictionary<TriggerType, List<SkillEntry>> _pipelines = new();

    public SkillEngine()
    {
        foreach (TriggerType t in Enum.GetValues<TriggerType>())
            _pipelines[t] = new();
    }
    public void RegisterAssembly(Assembly assembly, string source = "unknown")
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsDefined(typeof(GameAttribute), false)) continue;

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = method.GetCustomAttribute<CombatSkillAttribute>();
                if (attr == null) continue;

                var action = (Action<BattleContext>)Delegate.CreateDelegate(
                    typeof(Action<BattleContext>), method);

                _pipelines[attr.Trigger].Add(new SkillEntry
                {
                    Action = action,
                    Priority = attr.Priority,
                    Trigger = attr.Trigger,
                    Source = source
                });
            }
        }
    }
    public void LoadDll(string dllPath)
    {
        try
        {
            var asm = Assembly.LoadFrom(dllPath);
            RegisterAssembly(asm, Path.GetFileName(dllPath));
            Console.WriteLine($"[Engine] Loaded DLL: {dllPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to load DLL {dllPath}: {ex.Message}");
        }
    }
    public void LoadSourceCode(string csPath)
    {
        try
        {
            string code = File.ReadAllText(csPath);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var coreRef = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var systemRef = MetadataReference.CreateFromFile(typeof(Console).Assembly.Location);
            var coreLibRef = MetadataReference.CreateFromFile(typeof(BattleContext).Assembly.Location);

            var compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(csPath),
                new[] { syntaxTree },
                new[] { coreRef, systemRef, coreLibRef },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Select(d => d.GetMessage()));
                Console.WriteLine($"[ERROR] Compilation failed for {csPath}:\n{errors}");
                return;
            }

            ms.Seek(0, SeekOrigin.Begin);
            var asm = Assembly.Load(ms.ToArray());
            RegisterAssembly(asm, Path.GetFileName(csPath));
            Console.WriteLine($"[Engine] Compiled & loaded: {csPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to load source {csPath}: {ex.Message}");
        }
    }
    public void LoadPluginsFromDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Console.WriteLine($"[WARN] Plugin directory not found: {dirPath}");
            return;
        }
        foreach (var dll in Directory.GetFiles(dirPath, "*.dll"))
            LoadDll(dll);
        foreach (var cs in Directory.GetFiles(dirPath, "*.cs"))
            LoadSourceCode(cs);
    }
    
    public void ExecutePipeline(TriggerType trigger, BattleContext context)
    {
        var pipeline = _pipelines[trigger]
            .OrderByDescending(s => s.Priority) 
            .ToList();

        foreach (var skill in pipeline)
        {
            try
            {
                skill.Action(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Skill '{skill.Source}' threw: {ex.Message}");
            }
        }
    }
}