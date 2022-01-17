using System;

namespace Target
{
    internal class Offset
    {
        public Offset(float deltaX, float deltaY, float deltaZ)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            DeltaZ = deltaZ;

            Distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ + deltaZ);
        }

        public float DeltaX { get; }
        public float DeltaY { get; }
        public float DeltaZ { get; }
        public float Distance { get; }
    }
}
