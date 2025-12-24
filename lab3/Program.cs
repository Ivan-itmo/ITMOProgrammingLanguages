using System.Security.AccessControl;

namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hero knight = new Knight("Arthur", 100, 20, 10, new HealingPotion(20));
            Hero wizard = new Wizard("Harry", 98, 25, 8, new PoisonDagger(25));
            
            BattleManager.StartBattle(knight, wizard);
        }
    }
}
