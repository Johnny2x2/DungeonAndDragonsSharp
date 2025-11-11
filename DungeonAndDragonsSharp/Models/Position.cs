namespace DungeonAndDragonsSharp.Models
{
    /// <summary>
    /// Represents a position on the battlefield
    /// </summary>
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculate distance to another position (in squares)
        /// </summary>
        public int DistanceTo(Position other)
        {
            return Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
