using System.ComponentModel;

namespace SuperStarTrek
{
    internal enum Command
    {
        [Description("To set course")]
        NAV,

        [Description("For short range sensor scan")]
        SRS,

        [Description("For long range sensor scan")]
        LRS,

        [Description("To fire phasers")]
        PHA,

        [Description("To fire photon torpedoes")]
        TOR,

        [Description("To raise or lower shields")]
        SHE,

        [Description("For damage control reports")]
        DAM,

        [Description("To call on library-computer")]
        COM,

        [Description("To resign your command")]
        XXX
    }
}
