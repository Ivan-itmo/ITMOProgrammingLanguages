namespace Core;

public class BattleContext
{
    public int DamageDealt { get; set; }
    public UnitStats Attacker { get; set; } = new();
    public UnitStats Defender { get; set; } = new();
}