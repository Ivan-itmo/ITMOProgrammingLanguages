using System;
using System.Runtime.InteropServices;

namespace Lab3
{
    public class Knight : Hero
    {
        public Knight(string name, int hp, int attackPower, int defense, IArtifact? artifact = null)
            : base(name, hp, attackPower, defense, artifact)
        {
        }

        public override void SpecialAbility(Hero target)
        {
            int rawDamage = AttackPower - target.Defense;
            int damage = Math.Max(0, rawDamage * 2);
            target.TakeDamage(damage);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Name} использует особую способность: Рыцарский удар! Нанесено {damage} урона!");
                Console.ResetColor();
                Console.Beep(500, 300); // звук: низкий, мощный
            }
            else
            {
                // ANSI-цвет (зелёный) + системный звук для Linux/macOS
                Console.WriteLine($"\x1b[32m{Name} использует особую способность: Рыцарский удар! Нанесено {damage} урона!\x1b[0m");
                Console.WriteLine("\a"); // звуковой сигнал
            }
        }
    }
}