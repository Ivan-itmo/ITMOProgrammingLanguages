namespace Core;

public class CombatSkillAttribute : Attribute
{
    public TriggerType Trigger { get; }
    public int Priority { get; set; } 

    public CombatSkillAttribute(TriggerType trigger, int priority = 0)
    {
        Trigger = trigger;
        Priority = priority;
    }
}