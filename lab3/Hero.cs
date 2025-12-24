using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public abstract class Hero
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int AttackPower { get; set; }
        public int Defense {  get; set; }
        public IArtifact? Artifact { get; set; }

        public void UseArtifact(Hero target)
        {
            if (Artifact != null)
            {
                Artifact.Use(this, target);
            }
        } 
        
        public Hero(string name, int hp, int attackPower, int defense, IArtifact? artifact)
        {
            Name = name;
            HP = hp;
            AttackPower = attackPower;
            Defense = defense;
            Artifact = artifact;
        }

        public void TakeDamage(int damage)
        {
            HP = HP - damage >= 0 ? HP - damage : 0;
            Console.WriteLine($"{Name} takes {damage}, remaining HP: {HP}");
        }

        public void Attack(Hero target)
        {
            Console.WriteLine($"{Name} attacks {target.Name}");
            target.TakeDamage(Math.Max(0, this.AttackPower - target.Defense));
        }

        public abstract void SpecialAbility(Hero target);

        public void PrintInfo()
        {
            Console.WriteLine(this.ToString());
        }

        public override String ToString()
        {
            return $"${Name} - HP: {HP}, Attack: {AttackPower}, Defense: {Defense}";
        }
    }
}
