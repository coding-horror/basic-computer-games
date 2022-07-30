namespace Digits
{
    internal class Game
    {
        private readonly IReadWrite _io;
        private readonly IRandom _random;

        public Game(IReadWrite io, IRandom random)
        {
            _io = io;
            _random = random;
        }

        internal void Play()
        {
            _io.Write(Streams.Introduction);

            if (_io.ReadNumber(Prompts.ForInstructions) != 0)
            {
                _io.Write(Streams.Instructions);
            }

            do 
            {
                PlayOne();
            } while (_io.ReadNumber(Prompts.WantToTryAgain) == 1);

            _io.Write(Streams.Thanks);
        }

        private void PlayOne()
        {

        }
    }
}