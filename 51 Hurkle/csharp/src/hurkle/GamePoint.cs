namespace hurkle
{
    internal class GamePoint
    {
        public int X {get;init;}
        public int Y {get;init;}

        public CardinalDirection GetDirectionTo(GamePoint target)
        {   
            if(X == target.X)
            {
                if(Y > target.Y)
                {
                    return CardinalDirection.South;
                }
                else if(Y < target.Y)
                {
                    return CardinalDirection.North;
                }
                else
                {
                    return CardinalDirection.None;
                }
            }
            else if(X > target.X)
            {
                if(Y == target.Y)
                {
                    return CardinalDirection.West;
                }
                else if(Y > target.Y)
                {
                    return CardinalDirection.SouthWest;
                }
                else
                {
                    return CardinalDirection.NorthWest;
                }
            }
            else
            {
                if(Y == target.Y)
                {
                    return CardinalDirection.East;
                }
                else if(Y > target.Y)
                {
                    return CardinalDirection.SouthEast;
                }
                else{
                    return CardinalDirection.NorthEast;
                }
            }
        }
    }
}