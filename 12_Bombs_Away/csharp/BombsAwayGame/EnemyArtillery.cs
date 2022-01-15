namespace BombsAwayGame;

/// <summary>
/// Represents enemy artillery.
/// </summary>
/// <param name="Name">Name of artillery type.</param>
/// <param name="Accuracy">Accuracy of artillery. This is the `T` variable in the original BASIC.</param>
internal record class EnemyArtillery(string Name, int Accuracy);
