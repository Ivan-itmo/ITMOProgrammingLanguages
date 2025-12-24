namespace Vampire;

using Core;

[Game("Vampire Mechanics")]
public class VampireMechanics
{
    [CombatSkill(TriggerType.OnAttack, Priority = 10)]
    public static void Vampirism(BattleContext ctx)
    {
        int heal = ctx.DamageDealt / 2;
        ctx.Attacker.Hp += heal;
        Console.WriteLine($"[System] Vampirism: healed {heal} HP.");
    }

    [CombatSkill(TriggerType.PostBattle, Priority = 5)]
    public static void BloodThirst(BattleContext ctx)
    {
        ctx.Attacker.Hp += 10;
        Console.WriteLine($"[System] BloodThirst: +10 HP after battle.");
    }
}