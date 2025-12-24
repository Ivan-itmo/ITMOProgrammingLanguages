using System;
using System.IO;
using Core;
using Engine;

string solutionRoot = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..")
);
string pluginDir = Path.Combine(solutionRoot, "Plugins");

Console.WriteLine($"Looking for plugins in: {pluginDir}");

var engine = new SkillEngine();
engine.LoadPluginsFromDirectory(pluginDir);

var context = new BattleContext
{
    DamageDealt = 100,
    Attacker = new UnitStats { Hp = 50 },
    Defender = new UnitStats { Hp = 100 }
};

Console.WriteLine("\n=== OnAttack ===");
engine.ExecutePipeline(TriggerType.OnAttack, context);

Console.WriteLine("\n=== PostBattle ===");
engine.ExecutePipeline(TriggerType.PostBattle, context);

Console.WriteLine($"\nFinal Attacker HP: {context.Attacker.Hp}");