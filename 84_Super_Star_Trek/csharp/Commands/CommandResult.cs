namespace SuperStarTrek.Commands
{
    internal class CommandResult
    {
        public static readonly CommandResult Ok = new(false);
        public static readonly CommandResult GameOver = new(true);

        private CommandResult(bool isGameOver)
        {
            IsGameOver = isGameOver;
        }

        private CommandResult(float timeElapsed)
        {
            TimeElapsed = timeElapsed;
        }

        public bool IsGameOver { get; }
        public float TimeElapsed { get; }

        public static CommandResult Elapsed(float timeElapsed) => new(timeElapsed);
    }
}
