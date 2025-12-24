using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class HealingPotion : IArtifact
    {
        public int healing { get; set; }
        public HealingPotion(int healing)
        {
            this.healing = healing;
        }

        public void Use(Hero owner, Hero target)
        {
            owner.HP += healing;
            Console.WriteLine($"({owner.Name}) Healing using potion for {healing}HP");
        }
    }
}
