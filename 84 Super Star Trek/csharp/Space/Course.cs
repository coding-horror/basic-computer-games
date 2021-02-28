using System;

namespace SuperStarTrek.Space
{
    // Implements the course calculations from the original code:
    //     530 FORI=1TO9:C(I,1)=0:C(I,2)=0:NEXTI
    //     540 C(3,1)=-1:C(2,1)=-1:C(4,1)=-1:C(4,2)=-1:C(5,2)=-1:C(6,2)=-1
    //     600 C(1,2)=1:C(2,2)=1:C(6,1)=1:C(7,1)=1:C(8,1)=1:C(8,2)=1:C(9,2)=1
    //
    //     3110 X1=C(C1,1)+(C(C1+1,1)-C(C1,1))*(C1-INT(C1))
    //     3140 X2=C(C1,2)+(C(C1+1,2)-C(C1,2))*(C1-INT(C1))
    internal class Course
    {
        private static readonly (int DeltaX, int DeltaY)[] cardinals = new[]
        {
            (0, 1),
            (-1, 1),
            (-1, 0),
            (-1, -1),
            (0, -1),
            (1, -1),
            (1, 0),
            (1, 1),
            (0, 1)
        };

        public Course(double direction)
        {
            if (direction < 1 || direction > 9)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(direction),
                    direction,
                    "Must be between 1 and 9, inclusive.");
            }

            var cardinalDirection = (int)(direction - 1) % 8;
            var fractionalDirection = direction - (int)direction;

            var baseCardinal = cardinals[cardinalDirection];
            var nextCardinal = cardinals[cardinalDirection + 1];

            DeltaX = baseCardinal.DeltaX + (nextCardinal.DeltaX - baseCardinal.DeltaX) * fractionalDirection;
            DeltaY = baseCardinal.DeltaY + (nextCardinal.DeltaY - baseCardinal.DeltaY) * fractionalDirection;
        }

        public double DeltaX { get; }
        public double DeltaY { get; }
    }
}
