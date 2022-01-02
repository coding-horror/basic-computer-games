using System;
using Tower.Models;
using Tower.Resources;
using Tower.UI;

namespace Tower
{
    internal class Game
    {
        private readonly Towers _towers;
        private readonly TowerDisplay _display;
        private readonly int _optimalMoveCount;
        private int _moveCount;

        public Game(int diskCount)
        {
            _towers = new Towers(diskCount);
            _display = new TowerDisplay(_towers);
            _optimalMoveCount = (1 << diskCount) - 1;
        }

        public bool Play()
        {
            Console.Write(Strings.Instructions);

            Console.Write(_display);

            while (true)
            {
                if (!Input.TryReadNumber(Prompt.Disk, out int disk)) { return false; }

                if (!_towers.TryFindDisk(disk, out var from, out var message))
                {
                    Console.WriteLine(message);
                    continue;
                }

                if (!Input.TryReadNumber(Prompt.Needle, out var to)) { return false; }

                if (!_towers.TryMoveDisk(from, to))
                {
                    Console.Write(Strings.IllegalMove);
                    continue;
                }

                Console.Write(_display);

                var result = CheckProgress();
                if (result.HasValue) { return result.Value; }
            }
        }

        private bool? CheckProgress()
        {
            _moveCount++;

            if (_moveCount == 128)
            {
                Console.Write(Strings.TooManyMoves);
                return false;
            }

            if (_towers.Finished)
            {
                if (_moveCount == _optimalMoveCount)
                {
                    Console.Write(Strings.Congratulations);
                }

                Console.WriteLine(Strings.TaskFinished, _moveCount);

                return true;
            }

            return default;
        }
    }
}
