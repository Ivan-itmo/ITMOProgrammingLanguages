namespace Lab3;

public class PoisonDagger : IArtifact
{
    public int damage { get; set; }

    public PoisonDagger(int _damage)
    {
        this.damage = _damage;
    }
    
    public void Use(Hero owner, Hero target)
    {
        target.TakeDamage(damage);
        Console.WriteLine($"({owner.Name}) attacks ({target.Name}) using PoisonDagger for {damage} damage");
    }
    
}