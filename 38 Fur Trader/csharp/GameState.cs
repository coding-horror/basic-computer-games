using System;
using System.Collections.Generic;
using System.Text;

namespace FurTrader
{
    internal class GameState
    {
        internal bool GameOver { get; set; }

        internal double Savings { get; set; }        

        internal int ExpeditionCount { get; set; }        

        internal int UnasignedFurCount { get; set; }        

        internal int[] Pelts { get; private set; }

        internal int MinkPelts { get { return this.Pelts[0]; } set { this.Pelts[0] = value; } }
        internal int BeaverPelts { get { return this.Pelts[0]; } set { this.Pelts[0] = value; } }
        internal int ErminePelts { get { return this.Pelts[0]; } set { this.Pelts[0] = value; } }
        internal int FoxPelts { get { return this.Pelts[0]; } set { this.Pelts[0] = value; } }

        internal int SelectedFort { get; set; }

        internal GameState()
        {
            this.Savings = 600;
            this.ExpeditionCount = 0;
            this.UnasignedFurCount = 190;
            this.Pelts = new int[4];
            this.SelectedFort = 0;
        }

        internal void StartTurn()
        {
            this.SelectedFort = 0;              // reset to a default value
            this.UnasignedFurCount = 190;       // each turn starts with 190 furs
            this.Pelts = new int[4];            // reset pelts on each turn
        }
    }
}
