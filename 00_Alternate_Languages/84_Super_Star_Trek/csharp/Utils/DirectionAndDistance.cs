using System;
using SuperStarTrek.Space;

namespace SuperStarTrek.Utils
{
    internal class DirectionAndDistance
    {
        private readonly float _fromX;
        private readonly float _fromY;

        private DirectionAndDistance(float fromX, float fromY)
        {
            _fromX = fromX;
            _fromY = fromY;
        }

        internal static DirectionAndDistance From(Coordinates coordinates) => From(coordinates.X, coordinates.Y);

        internal static DirectionAndDistance From(float x, float y) => new DirectionAndDistance(x, y);

        internal (float Direction, float Distance) To(Coordinates coordinates) => To(coordinates.X, coordinates.Y);

        internal (float Direction, float Distance) To(float x, float y)
        {
            var deltaX = x - _fromX;
            var deltaY = y - _fromY;

            return (GetDirection(deltaX, deltaY), GetDistance(deltaX, deltaY));
        }

        // The algorithm here is mathematically equivalent to the following code in the original,
        // where X is deltaY and A is deltaX
        //     8220 X=X-A:A=C1-W1:IFX<0THEN8350
        //     8250 IFA<0THEN8410
        //     8260 IFX>0THEN8280
        //     8270 IFA=0THENC1=5:GOTO8290
        //     8280 C1=1
        //     8290 IFABS(A)<=ABS(X)THEN8330
        //     8310 PRINT"DIRECTION =";C1+(((ABS(A)-ABS(X))+ABS(A))/ABS(A)):GOTO8460
        //     8330 PRINT"DIRECTION =";C1+(ABS(A)/ABS(X)):GOTO8460
        //     8350 IFA>0THENC1=3:GOTO8420
        //     8360 IFX<>0THENC1=5:GOTO8290
        //     8410 C1=7
        //     8420 IFABS(A)>=ABS(X)THEN8450
        //     8430 PRINT"DIRECTION =";C1+(((ABS(X)-ABS(A))+ABS(X))/ABS(X)):GOTO8460
        //     8450 PRINT"DIRECTION =";C1+(ABS(X)/ABS(A))
        //     8460 PRINT"DISTANCE =";SQR(X^2+A^2):IFH8=1THEN1990
        private static float GetDirection(float deltaX, float deltaY)
        {
            var deltaXDominant = Math.Abs(deltaX) > Math.Abs(deltaY);
            var fractionalPart = deltaXDominant ? deltaY / deltaX : -deltaX / deltaY;
            var nearestCardinal = deltaXDominant switch
            {
                true => deltaX > 0 ? 7 : 3,
                false => deltaY > 0 ? 1 : 5
            };

            var direction = nearestCardinal + fractionalPart;
            return direction < 1 ? direction + 8 : direction;
        }

        private static float GetDistance(float deltaX, float deltaY) =>
            (float)Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
    }
}
