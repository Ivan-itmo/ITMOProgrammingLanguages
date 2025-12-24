using System;
using System.IO;

namespace Lab3
{
    public class BattleManager
    {
        public static void StartBattle(Hero hero1, Hero hero2)
        {
            Console.WriteLine($"Начало боя: {hero1.Name} vs {hero2.Name}\n");

            int turn = 1;
            Random random = new Random();

            while (hero1.HP > 0 && hero2.HP > 0)
            {
                Hero attacker = (turn % 2 == 1) ? hero1 : hero2;
                Hero defender = (attacker == hero1) ? hero2 : hero1;

                Console.WriteLine($"=== Ход {turn} ===");
                Console.WriteLine($"{attacker.Name} атакует {defender.Name}");

                if (turn % 3 == 0)
                {
                    attacker.SpecialAbility(defender);
                }
                else
                {
                    if (attacker.Artifact != null && random.Next(4) == 0)
                    {
                        Console.WriteLine($"{attacker.Name} использует артефакт!");
                        attacker.Artifact.Use(attacker, defender);
                    }
                    else
                    {
                        attacker.Attack(defender);
                    }
                }
                Console.WriteLine($"{hero1.Name}: {Math.Max(0, hero1.HP)} HP | {hero2.Name}: {Math.Max(0, hero2.HP)} HP\n");

                turn++;
            }
            Hero winner = hero1.HP > 0 ? hero1 : hero2;
            Console.WriteLine($"🏆 Победитель: {winner.Name}!");

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string logPath = Path.Combine(docPath, "battle_log.txt");
            string logMessage = $"Битва завершена! Победитель: {winner.Name} с {Math.Max(0, winner.HP)} HP.";
            
            File.WriteAllText(logPath, logMessage);
            Console.WriteLine($"Лог боя сохранён: {logPath}");
        }
    }
}