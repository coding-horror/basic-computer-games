using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class Bowling
    {
        private readonly Pins pins = new Pins();

        private int players;

        public void Play()
        {
            ShowBanner();
            MaybeShowInstructions();
            Setup();
            GameLoop();
        }

        private static void ShowBanner()
        {
            Utility.PrintString(34, "BOWL");
            Utility.PrintString(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Utility.PrintString();
            Utility.PrintString();
            Utility.PrintString();
            Utility.PrintString("WELCOME TO THE ALLEY");
            Utility.PrintString("BRING YOUR FRIENDS");
            Utility.PrintString("OKAY LET'S FIRST GET ACQUAINTED");
            Utility.PrintString();
        }
        private static void MaybeShowInstructions()
        { 
            Utility.PrintString("THE INSTRUCTIONS (Y/N)");
            if (Utility.InputString() == "N") return;
            Utility.PrintString("THE GAME OF BOWLING TAKES MIND AND SKILL.DURING THE GAME");
            Utility.PrintString("THE COMPUTER WILL KEEP SCORE.YOU MAY COMPETE WITH");
            Utility.PrintString("OTHER PLAYERS[UP TO FOUR].YOU WILL BE PLAYING TEN FRAMES");
            Utility.PrintString("ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE");
            Utility.PrintString("PIN IS STANDING.AFTER THE GAME THE COMPUTER WILL SHOW YOUR");
            Utility.PrintString("SCORES .");
        }
        private void Setup()
        {
            Utility.PrintString("FIRST OF ALL...HOW MANY ARE PLAYING", false);
            var input = Utility.InputInt();
            players = input < 1 ? 1 : input;
            Utility.PrintString();
            Utility.PrintString("VERY GOOD...");
        }
        private void GameLoop()
        {
            var gameResults = new GameResults[players];
            var done = false;
            while (!done)
            {
                ResetGameResults(gameResults);
                for (int frame = 0; frame < GameResults.FramesPerGame; ++frame)
                {
                    for (int player = 0; player < players; ++player)
                    {
                        pins.Reset();
                        int pinsDownThisFrame = pins.GetPinsDown();

                        int ball = 1;
                        while (ball == 1 || ball == 2) // One or two rolls
                        {
                            Utility.PrintString("TYPE ROLL TO GET THE BALL GOING.");
                            _ = Utility.InputString();

                            int pinsDownAfterRoll = pins.Roll();
                            ShowPins(player, frame, ball);

                            if (pinsDownAfterRoll == pinsDownThisFrame)
                            {
                                Utility.PrintString("GUTTER!!");
                            }

                            if (ball == 1)
                            {
                                // Store current pin count
                                gameResults[player].Results[frame].PinsBall1 = pinsDownAfterRoll;

                                // Special handling for strike
                                if (pinsDownAfterRoll == Pins.TotalPinCount)
                                {
                                    Utility.PrintString("STRIKE!!!!!\a\a\a\a");
                                    // No second roll
                                    ball = 0; 
                                    gameResults[player].Results[frame].PinsBall2 = pinsDownAfterRoll;
                                    gameResults[player].Results[frame].Score = FrameResult.Points.Strike;
                                }
                                else
                                {
                                    ball = 2; // Roll again
                                    Utility.PrintString("ROLL YOUR SECOND BALL");
                                }
                            }
                            else if (ball == 2)
                            {
                                // Store current pin count
                                gameResults[player].Results[frame].PinsBall2 = pinsDownAfterRoll;
                                ball = 0;

                                // Determine the score for the frame
                                if (pinsDownAfterRoll == Pins.TotalPinCount)
                                {
                                    Utility.PrintString("SPARE!!!!");
                                    gameResults[player].Results[frame].Score = FrameResult.Points.Spare;
                                }
                                else
                                {
                                    Utility.PrintString("ERROR!!!");
                                    gameResults[player].Results[frame].Score = FrameResult.Points.Error;
                                }
                            }
                            Utility.PrintString();
                        }
                    }
                }
                ShowGameResults(gameResults);
                Utility.PrintString("DO YOU WANT ANOTHER GAME");
                var a = Utility.InputString();
                done = a.Length == 0 || a[0] != 'Y';
            }
        }
        private void ShowPins(int player, int frame, int ball)
        {
            Utility.PrintString($"FRAME: {frame + 1} PLAYER: {player + 1} BALL: {ball}");
            var breakPins = new bool[] { true, false, false, false, true, false, false, true, false, true };
            var indent = 0;
            for (int pin = 0; pin < Pins.TotalPinCount; ++pin)
            {
                if (breakPins[pin])
                {
                    Utility.PrintString(); // End row
                    Utility.PrintString(indent++, false); // Indent next row
                }
                var s = pins[pin] == Pins.State.Down ? "+ " : "o ";
                Utility.PrintString(s, false);
            }
            Utility.PrintString();
            Utility.PrintString();
        }
        private void ResetGameResults(GameResults[] gameResults)
        {
            for (int playerIndex = 0; playerIndex < gameResults.Length; ++playerIndex)
            {
                var frameResults = gameResults[playerIndex];
                if (frameResults == null)
                {
                    gameResults[playerIndex] = new GameResults();
                }
                else
                {
                    for (int frameIndex = 0; frameIndex < frameResults.Results.Length; ++frameIndex)
                    {
                        gameResults[playerIndex].Results[frameIndex].Reset();
                    }
                }
            }
        }
        private void ShowGameResults(GameResults[] gameResults)
        {
            Utility.PrintString("FRAMES");
            for (int i = 0; i < GameResults.FramesPerGame; ++i)
            {
                Utility.PrintString(Utility.PadInt(i, 3), false);
            }
            Utility.PrintString();
            for (var player = 0; player < gameResults.Length; ++player)
            {
                for (var frame = 0; frame < gameResults[player].Results.Length; ++frame)
                {
                    Utility.PrintString(Utility.PadInt(gameResults[player].Results[frame].PinsBall1, 3), false);
                }
                Utility.PrintString();
                for (var frame = 0; frame < gameResults[player].Results.Length; ++frame)
                {
                    Utility.PrintString(Utility.PadInt(gameResults[player].Results[frame].PinsBall2, 3), false);
                }
                Utility.PrintString();
                for (var frame = 0; frame < gameResults[player].Results.Length; ++frame)
                {
                    Utility.PrintString(Utility.PadInt((int)gameResults[player].Results[frame].Score, 3), false);
                }
                Utility.PrintString();
                Utility.PrintString();
            }
        }
    }
}
