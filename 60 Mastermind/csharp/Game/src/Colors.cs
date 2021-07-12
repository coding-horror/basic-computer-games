namespace Game
{
    /// <summary>
    /// Provides information about the colors that can be used in codes.
    /// </summary>
    public static class Colors
    {
        public static readonly ColorInfo[] List = new[]
        {
            new ColorInfo { ShortName = 'B', LongName = "BLACK"  },
            new ColorInfo { ShortName = 'W', LongName = "WHITE"  },
            new ColorInfo { ShortName = 'R', LongName = "RED"    },
            new ColorInfo { ShortName = 'G', LongName = "GREEN"  },
            new ColorInfo { ShortName = 'O', LongName = "ORANGE" },
            new ColorInfo { ShortName = 'Y', LongName = "YELLOW" },
            new ColorInfo { ShortName = 'P', LongName = "PURPLE" },
            new ColorInfo { ShortName = 'T', LongName = "TAN"    }
        };
    }
}
