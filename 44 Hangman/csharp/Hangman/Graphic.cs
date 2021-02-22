using System;

namespace Hangman
{
    /// <summary>
    /// Represents the main "Hangman" graphic.
    /// </summary>
    public class Graphic
    {
        private readonly char[,] _graphic;
        private const int Width = 12;
        private const int Height = 12;

        public Graphic()
        {
            // 12 x 12 array to represent the graphics.
            _graphic = new char[Height, Width];

            // Fill it with empty spaces.
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    _graphic[i, j] = ' ';
                }
            }

            // Draw the vertical line.
            for (var i = 0; i < Height; i++)
            {
                _graphic[i, 0] = 'X';
            }

            // Draw the horizontal line.
            for (var i = 0; i < 7; i++)
            {
                _graphic[0, i] = 'X';
            }

            // Draw the rope.
            _graphic[1, 6] = 'X';
        }

        public void Print()
        {
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    Console.Write(_graphic[i, j]);
                }

                Console.Write("\n"); // New line.
            }
        }

        public void AddHead()
        {
            _graphic[2, 5] = '-';
            _graphic[2, 6] = '-';
            _graphic[2, 7] = '-';
            _graphic[3, 4] = '(';
            _graphic[3, 5] = '.';
            _graphic[3, 7] = '.';
            _graphic[3, 8] = ')';
            _graphic[4, 5] = '-';
            _graphic[4, 6] = '-';
            _graphic[4, 7] = '-';
        }

        public void AddBody()
        {
            for (var i = 5; i < 9; i++)
            {
                _graphic[i, 6] = 'X';
            }
        }

        public void AddRightArm()
        {
            for (var i = 3; i < 7; i++)
            {
                _graphic[i, i - 1] = '\\'; // This is the escape character for the back slash.
            }
        }

        public void AddLeftArm()
        {
            _graphic[3, 10] = '/';
            _graphic[4, 9] = '/';
            _graphic[5, 8] = '/';
            _graphic[6, 7] = '/';
        }

        public void AddRightLeg()
        {
            _graphic[9, 5] = '/';
            _graphic[10, 4] = '/';
        }

        public void AddLeftLeg()
        {
            _graphic[9, 7] = '\\';
            _graphic[10, 8] = '\\';
        }

        public void AddRightHand()
        {
            _graphic[2, 2] = '/';
        }
        
        public void AddLeftHand()
        {
            _graphic[2, 10] = '\\';
        }
        
        public void AddRightFoot()
        {
            _graphic[11, 9] = '\\';
            _graphic[11, 10] = '-';
        }
        
        public void AddLeftFoot()
        {
            _graphic[11, 3] = '/';
            _graphic[11, 2] = '-';
        }
    }
}