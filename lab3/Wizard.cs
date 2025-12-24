using System;
using System.Runtime.InteropServices;

namespace Lab3
{
    public class Wizard : Hero
    {
        public Wizard(string name, int hp, int attackPower, int defense, IArtifact? artifact = null)
            : base(name, hp, attackPower, defense, artifact)
        {
        }

        public override void SpecialAbility(Hero target)
        {
            // Магическая атака игнорирует защиту — урон = AttackPower
            int damage = AttackPower;
            target.TakeDamage(damage);

            // Цветной вывод и звук в зависимости от ОС
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{Name} кастует Огненный шар! Нанесено {damage} урона!");
                Console.ResetColor();
                Console.Beep(900, 200); // высокий звук — магия
            }
            else
            {
                // ANSI-код для фиолетового/розового текста
                Console.WriteLine($"\x1b[35m{Name} кастует Огненный шар! Нанесено {damage} урона!\x1b[0m");
                Console.WriteLine("\a"); // системный звук
            }
        }
    }
}