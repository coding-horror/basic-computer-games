using System;
using Tower.Resources;
using Tower.UI;

namespace Tower
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(Strings.Title);

            do
            {
                Console.Write(Strings.Intro);

                if (!Input.TryReadNumber(Prompt.DiskCount, out var diskCount)) { return; }

                var game = new Game(diskCount);

                if (!game.Play()) { return; }
            } while (Input.ReadYesNo(Strings.PlayAgainPrompt, Strings.YesNoPrompt));

            Console.Write(Strings.Thanks);
        }
    }
}
