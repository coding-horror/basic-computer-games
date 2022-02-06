using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    public class FrameResult
    {
        public enum Points { None, Error, Spare, Strike };

        public int PinsBall1 { get; set; }
        public int PinsBall2 { get; set; }
        public Points Score { get; set; }

        public void Reset()
        {
            PinsBall1 = PinsBall2 = 0;
            Score = Points.None;
        }
    }
}
