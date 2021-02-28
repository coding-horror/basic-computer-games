
using System;
using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class ShortRangeSensors : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly Galaxy _galaxy;
        private readonly Game _game;
        private readonly Output _output;

        public ShortRangeSensors(Enterprise enterprise, Galaxy galaxy, Game game, Output output)
            : base("Short Range Sensors", Command.SRS)
        {
            _enterprise = enterprise;
            _galaxy = galaxy;
            _game = game;
            _output = output;
        }

        public override void ExecuteCommand(Quadrant quadrant)
        {
            if (_enterprise.IsDocked)
            {
                _output.WriteLine(Strings.ShieldsDropped);
            }

            if (Condition < 0)
            {
                _output.WriteLine(Strings.ShortRangeSensorsOut);
            }

            _output.WriteLine("---------------------------------");
            quadrant.GetDisplayLines()
                .Zip(GetStatusLines(), (sectors, status) => $" {sectors}         {status}")
                .ToList()
                .ForEach(l => _output.WriteLine(l));
            _output.WriteLine("---------------------------------");
        }

        public IEnumerable<string> GetStatusLines()
        {
            yield return $"Stardate           {_game.Stardate}";
            yield return $"Condition          {_enterprise.Condition}";
            yield return $"Quadrant           {_enterprise.Quadrant}";
            yield return $"Sector             {_enterprise.Sector}";
            yield return $"Photon torpedoes   {_enterprise.TorpedoCount}";
            yield return $"Total energy       {Math.Ceiling(_enterprise.Energy)}";
            yield return $"Shields            {(int)_enterprise.Shields}";
            yield return $"Klingons remaining {_galaxy.KlingonCount}";
        }
    }
}
