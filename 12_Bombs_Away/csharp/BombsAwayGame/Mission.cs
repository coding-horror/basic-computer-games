namespace BombsAwayGame;

/// <summary>
/// Represents a mission that can be flown by a <see cref="MissionSide"/>.
/// </summary>
/// <param name="Name">Name of mission.</param>
/// <param name="Description">Description of mission.</param>
internal record class Mission(string Name, string Description);
