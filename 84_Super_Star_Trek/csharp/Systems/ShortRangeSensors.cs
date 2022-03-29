
using System;
using System.Collections.Generic;
using System.Linq;
using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems;

internal class ShortRangeSensors : Subsystem
{
    private readonly Enterprise _enterprise;
    private readonly Galaxy _galaxy;
    private readonly Game _game;
    private readonly IReadWrite _io;

    internal ShortRangeSensors(Enterprise enterprise, Galaxy galaxy, Game game, IReadWrite io)
        : base("Short Range Sensors", Command.SRS, io)
    {
        _enterprise = enterprise;
        _galaxy = galaxy;
        _game = game;
        _io = io;
    }

    protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
    {
        if (_enterprise.IsDocked)
        {
            _io.WriteLine(Strings.ShieldsDropped);
        }

        if (Condition < 0)
        {
            _io.WriteLine(Strings.ShortRangeSensorsOut);
        }

        _io.WriteLine("---------------------------------");
        quadrant.GetDisplayLines()
            .Zip(GetStatusLines(), (sectors, status) => $" {sectors}         {status}")
            .ToList()
            .ForEach(l => _io.WriteLine(l));
        _io.WriteLine("---------------------------------");

        return CommandResult.Ok;
    }

    internal IEnumerable<string> GetStatusLines()
    {
        yield return $"Stardate           {_game.Stardate}";
        yield return $"Condition          {_enterprise.Condition}";
        yield return $"Quadrant           {_enterprise.QuadrantCoordinates}";
        yield return $"Sector             {_enterprise.SectorCoordinates}";
        yield return $"Photon torpedoes   {_enterprise.PhotonTubes.TorpedoCount}";
        yield return $"Total energy       {Math.Ceiling(_enterprise.TotalEnergy)}";
        yield return $"Shields            {(int)_enterprise.ShieldControl.ShieldEnergy}";
        yield return $"Klingons remaining {_galaxy.KlingonCount}";
    }
}
