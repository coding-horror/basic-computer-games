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

        private CommandResult(double timeElapsed)
        {
            TimeElapsed = timeElapsed;
        }

        public bool IsGameOver { get; }
        public double TimeElapsed { get; }

        public static CommandResult Elapsed(double timeElapsed) => new(timeElapsed);
    }
}
