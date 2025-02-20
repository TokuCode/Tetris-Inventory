using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static class CollissionHandler
    {
        public static bool IsCollidingWith(this Shape shape, Shape other)
        {
            bool xOverlapLeftSided = shape.X + shape.Width >= other.X && shape.X + shape.Width <= other.X + other.Width;
            bool xOverlapRightSided = shape.X >= other.X && shape.X <= other.X + other.Width;
            bool xInside = shape.X >= other.X && shape.X + shape.Width <= other.X + other.Width;
            bool xOtherInside = other.X >= shape.X && other.X + other.Width <= shape.X + shape.Width;
            
            bool yOverlapTopSided = shape.Y + shape.Height >= other.Y && shape.Y + shape.Height <= other.Y + other.Height;
            bool yOverlapBottomSided = shape.Y >= other.Y && shape.Y <= other.Y + other.Height;
            bool yInside = shape.Y >= other.Y && shape.Y + shape.Height <= other.Y + other.Height;
            bool yOtherInside = other.Y >= shape.Y && other.Y + other.Height <= shape.Y + shape.Height;
            
            bool overlapX = xOverlapLeftSided || xOverlapRightSided || xInside || xOtherInside;
            bool overlapY = yOverlapTopSided || yOverlapBottomSided || yInside || yOtherInside;
            
            if(!(overlapX && overlapY)) return false;

            int overlapX1 = shape.X < other.X ? other.X : shape.X;
            int overlapY1 = shape.Y < other.Y ? other.Y : shape.Y;
            int overlapX2 = shape.X + shape.Width > other.X + other.Width ? other.X + other.Width : shape.X + shape.Width;
            int overlapY2 = shape.Y + shape.Height > other.Y + other.Height ? other.Y + other.Height : shape.Y + shape.Height;

            int overlapWidth = overlapX2 - overlapX1;
            int overlapHeight = overlapY2 - overlapY1;
            
            for(int x = 0; x < overlapWidth; x++)
            for (int y = 0; y < overlapHeight; y++)
            {
                bool val1 = shape[overlapX1 - shape.X + x, overlapY1 - shape.Y + y];
                bool val2 = other[overlapX1 - other.X + x, overlapY1 - other.Y + y];
                
                if(val1 && val2) return true;
            }

            return false;
        }
        
        public static bool AreColliding(this Shape[] shapes)
        {
            for (int i = 0; i < shapes.Length; i++)
            for (int j = i + 1; j < shapes.Length; j++)
            {
                if (shapes[i].IsCollidingWith(shapes[j]))
                    return true;
            }

            return false;
        }

        public static bool IsInsideOf(this Shape shape, Shape container)
        {
            bool x = shape.X >= container.X && shape.X + shape.Width <= container.X + container.Width;
            bool y = shape.Y >= container.Y && shape.Y + shape.Height <= container.Y + container.Height;
            
            return x && y;
        }

        public static bool FitInsideOf(this Shape shape, Shape container, params Shape[] others)
        {
            var shapes = new List<Shape>(others) {shape, container};
            
            return !AreColliding(shapes.ToArray()) && shape.IsInsideOf(container);
        }
    }
}