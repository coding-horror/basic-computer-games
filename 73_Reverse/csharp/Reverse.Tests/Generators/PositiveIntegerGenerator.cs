using FsCheck;

namespace Reverse.Tests.Generators
{
    public static class PositiveIntegerGenerator
    {
        public static Arbitrary<int> Generate() =>
            Arb.Default.Int32().Filter(x => x > 0);
    }
}
