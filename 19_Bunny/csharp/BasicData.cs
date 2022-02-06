using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunny
{
    internal class BasicData
    {
        private readonly int[] data;

        private int index;

        public BasicData(int[] data)
        {
            this.data = data;
            index = 0;
        }
        public int Read()
        {
            return data[index++];
        }
    }
}
