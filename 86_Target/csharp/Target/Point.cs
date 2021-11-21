using System;

namespace Target
{
    internal class Point
    {
        private readonly float _angleFromX;
        private readonly float _angleFromZ;

        private readonly float _x;
        private readonly float _y;
        private readonly float _z;

        private int _estimateCount;

        public Point(Angle angleFromX, Angle angleFromZ, float distance)
        {
            _angleFromX = angleFromX;
            _angleFromZ = angleFromZ;
            Distance = distance;

            _x = distance * (float)Math.Sin(_angleFromZ) * (float)Math.Cos(_angleFromX);
            _y = distance * (float)Math.Sin(_angleFromZ) * (float)Math.Sin(_angleFromX);
            _z = distance * (float)Math.Cos(_angleFromZ);
        }

        public float Distance { get; }

        public float EstimateDistance() =>
            ++_estimateCount switch
            {
                1 => EstimateDistance(20),
                2 => EstimateDistance(10),
                3 => EstimateDistance(5),
                4 => EstimateDistance(1),
                _ => Distance
            };

        public float EstimateDistance(int precision) => (float)Math.Floor(Distance / precision) * precision;

        public string GetBearing() => $"Radians from X axis = {_angleFromX}   from Z axis = {_angleFromZ}";

        public override string ToString() => $"X= {_x}   Y = {_y}   Z= {_z}";

        public static Offset operator -(Point p1, Point p2) => new (p1._x - p2._x, p1._y - p2._y, p1._z - p2._z);
    }
}
