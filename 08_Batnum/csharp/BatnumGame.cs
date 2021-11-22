using Batnum.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batnum
{
    public enum WinOptions
    {
        /// <summary>
        /// Last person to play wins
        /// </summary>
        WinWithTakeLast = 1,
        /// <summary>
        /// Last person to play loses
        /// </summary>
        WinWithAvoidLast = 2
    }

    public enum Players
    {
        Computer = 1,
        Human = 2
    }

    public class BatnumGame
    {
        public BatnumGame(int pileSize, WinOptions winCriteria, int minTake, int maxtake, Players firstPlayer, Func<string, int>askPlayerCallback)
        {
            this.pileSize = pileSize;
            this.winCriteria = winCriteria;
            this.minTake = minTake;
            this.maxTake = maxtake;
            this.currentPlayer = firstPlayer;
            this.askPlayerCallback = askPlayerCallback;
        }

        private int pileSize;
        private WinOptions winCriteria;
        private int minTake;
        private int maxTake;
        private Players currentPlayer;
        private Func<string, int> askPlayerCallback;

        /// <summary>
        /// Returns true if the game is running
        /// </summary>
        public bool IsRunning => pileSize > 0;

        /// <summary>
        /// Takes the next turn
        /// </summary>
        /// <returns>A message to be displayed to the player</returns>
        public string TakeTurn()
        {
            //Edge condition - can occur when minTake is more > 1 
            if (pileSize < minTake)
            {
                pileSize = 0;
                return string.Format(Resources.END_DRAW, minTake);
            }
            return currentPlayer == Players.Computer ? ComputerTurn() : PlayerTurn();
        }

        private string PlayerTurn()
        {
            int draw = askPlayerCallback(Resources.INPUT_TURN);
            if (draw == 0)
            {
                pileSize = 0;
                return Resources.INPUT_ZERO;
            }
            if (draw < minTake || draw > maxTake || draw > pileSize)
            {
                return Resources.INPUT_ILLEGAL;
            }
            pileSize = pileSize - draw;
            if (pileSize == 0)
            {
                return winCriteria == WinOptions.WinWithTakeLast ? Resources.END_PLAYERWIN : Resources.END_PLAYERLOSE;
            }
            currentPlayer = Players.Computer;
            return "";
        }

        private string ComputerTurn()
        {
            //first calculate the move to play
            int sumTake = minTake + maxTake;
            int draw = pileSize - sumTake * (int)(pileSize / (float)sumTake);
            draw = Math.Clamp(draw, minTake, maxTake);

            //detect win/lose conditions
            switch (winCriteria)
            {
                case WinOptions.WinWithAvoidLast when (pileSize == minTake): //lose condition
                    pileSize = 0;
                    return string.Format(Resources.END_COMPLOSE, minTake);
                case WinOptions.WinWithAvoidLast when (pileSize <= maxTake): //avoid automatic loss on next turn
                    draw = Math.Clamp(draw, minTake, pileSize - 1);
                    break;
                case WinOptions.WinWithTakeLast when pileSize <= maxTake: // win condition
                    draw = Math.Min(pileSize, maxTake);
                    pileSize = 0;
                    return string.Format(Resources.END_COMPWIN, draw);
            }
            pileSize -= draw;
            currentPlayer = Players.Human;
            return string.Format(Resources.COMPTURN, draw, pileSize);
        }
    }
}
