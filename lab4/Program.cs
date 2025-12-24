using Lab4;
using CsvHelper;
using System;
using System.IO;
using System.Linq;

namespace ProgLang4
{
    class Program
    {
        static void Main(string[] args)
        {
            using var reader = new StreamReader("tmdb_5000_credits.csv");
            using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<MovieMap>();
            var records = csv.GetRecords<Movie>().ToList();

            Console.WriteLine("Данные загружены. Всего фильмов: " + records.Count);
            Console.WriteLine();
            Console.WriteLine("Пример: должности в Crew первого фильма:");
            var crew = records.First().Crew
                .Select(member => member.Job)
                .Distinct()
                .ToList();
            Console.WriteLine("Crew: " + string.Join(", ", crew));
            Console.WriteLine();
            
            Console.WriteLine("1. Фильмы Quentin Tarantino:");
            var tarantinoMovies = records
                .Where(movie => movie.Crew.Any(c => 
                    c.Job.Equals("Director", StringComparison.OrdinalIgnoreCase) &&
                    c.Name.Equals("Quentin Tarantino", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (tarantinoMovies.Any())
                foreach (var m in tarantinoMovies)
                    Console.WriteLine($"  • {m.Title}");
            else
                Console.WriteLine("  Не найдено");
            Console.WriteLine();
            
            Console.WriteLine("2. Топ-5 фильмов с наибольшим актёрским составом:");
            var top5 = records
                .OrderByDescending(m => m.Cast.Count)
                .Take(5)
                .ToList();

            foreach (var m in top5)
                Console.WriteLine($"  • {m.Title} — {m.Cast.Count} актёров");
            Console.WriteLine();
            
            Console.WriteLine("3. Режиссёр с наибольшим числом фильмов:");
            var topDirector = records
                .SelectMany(m => m.Crew.Where(c => 
                    c.Job.Equals("Director", StringComparison.OrdinalIgnoreCase)))
                .GroupBy(d => d.Name)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (topDirector != null)
                Console.WriteLine($"{topDirector.Key} — {topDirector.Count()} фильмов");
            else
                Console.WriteLine("  Не найдено");
            Console.WriteLine();
        }
    }
}