using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivilWar
{
    static class ConsoleUtils
    {
        public static bool InputYesOrNo()
        {
            while (true)
            {
                var answer = Console.ReadLine();
                switch (answer?.ToLower())
                {
                    case "no":
                        return false;
                    case "yes":
                        return true;
                    default:
                        Console.WriteLine("(Answer Yes or No)");
                        break;
                }
            }
        }

        public static void WriteWordWrap(string text)
        {
            var line = new StringBuilder(Console.WindowWidth);
            foreach (var paragraph in text.Split(Environment.NewLine))
            {
                line.Clear();
                foreach (var word in paragraph.Split(' '))
                {
                    if (line.Length + word.Length < Console.WindowWidth)
                    {
                        if (line.Length > 0)
                            line.Append(' ');
                        line.Append(word);
                    }
                    else
                    {
                        Console.WriteLine(line.ToString());
                        line.Clear().Append(word);
                    }
                }
                Console.WriteLine(line.ToString());
            }
        }

        public static void WriteTable<T>(ICollection<T> items, List<TableRow<T>> rows, bool transpose = false)
        {
            int cols = items.Count + 1;
            var content = rows.Select(r => r.Format(items)).ToList();
            if (transpose)
            {
                content = Enumerable.Range(0, cols).Select(col => content.Select(r => r[col]).ToArray()).ToList();
                cols = rows.Count;
            }
            var colWidths = Enumerable.Range(0, cols).Select(col => content.Max(c => c[col].Length)).ToArray();

            foreach (var row in content)
            {
                for (int col = 0; col < cols; col++)
                {
                    var space = new string(' ', colWidths[col] - row[col].Length);
                    row[col] = col == 0 ? row[col] + space : space + row[col]; // left-align first col; right-align other cols
                }
            }

            var sb = new StringBuilder();
            var horizBars = colWidths.Select(w => new string('═', w)).ToArray();

            void OneRow(string[] cells, char before, char between, char after)
            {
                sb.Append(before);
                sb.AppendJoin(between, cells);
                sb.Append(after);
                sb.AppendLine();
            }

            OneRow(horizBars, '╔', '╦', '╗');
            bool first = true;
            foreach (var row in content)
            {
                if (first)
                    first = false;
                else
                    OneRow(horizBars, '╠', '╬', '╣');
                OneRow(row, '║', '║', '║');
            }
            OneRow(horizBars, '╚', '╩', '╝');

            Console.WriteLine(sb.ToString());
        }

        public record TableRow<T>(string Name, Func<T, object> Data, string Before = "", string After = "")
        {
            private string FormatItem(T item) => $" {Before}{Data(item)}{After} ";

            public string[] Format(IEnumerable<T> items) => items.Select(FormatItem).Prepend($" {Name} ").ToArray();
        }
    }
}
