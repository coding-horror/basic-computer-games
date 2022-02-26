using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class GameResults
    {
        public static readonly int FramesPerGame = 10;
        public FrameResult[] Results { get; set; }

        public GameResults()
        {
            Results = new FrameResult[FramesPerGame];
            for (int i = 0; i < FramesPerGame; ++i)
            {
                Results[i] = new FrameResult();
            }
        }
    }
}
