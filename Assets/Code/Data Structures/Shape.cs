using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems
{
    [Serializable]
    public class Shape : ICloneable
    {
        [SerializeField, HideInInspector] private FalseMatrix<bool> details;
        
        public bool this[int x, int y]
        {
            get => details[x, y];
            set => details[x, y] = value;
        }
        
        public bool this[int index]
        {
            get => details[index];
            set => details[index] = value;
        }

        [SerializeField, HideInInspector] private Box bounds;
        
        public int X => bounds.X;
        public int Y => bounds.Y;
        public int Width => bounds.Width;
        public int Height => bounds.Height;
        public int Area => Width * Height;
        public int Count => details.Count;
        public int FreeSpace => Area - Count;

        public event Action OnShapeChange;
        public void Invoke() => OnShapeChange?.Invoke();
        
        public Shape(Box bounds, bool[] initialShape = null)
        {
            this.bounds = bounds;
            details = new FalseMatrix<bool>(Width, Height, initialShape, new BoolCounter());
        }
        
        public object Clone()
        {
            return new Shape(bounds, details.array);
        }

        #region ShapeOperations

        public void Move(int x, int y, bool relative = true)
        {
            if (relative) (x, y) = (x + bounds.X, y + bounds.Y);

            bounds = new Box()
            {
                X = x,
                Y = y,
                Width = Width,
                Height = Height
            };
            Invoke();
        }

        public void Rotate(int quarters, bool clockwise = true)
        {
            if (quarters < 0) quarters *= -1;
            int direction = clockwise ? 1 : -1;
            int rotation = quarters * direction % 4;
            if (rotation < 0) rotation += 4;
            
            for(int i = 0; i < rotation; i++) RotateQuarterClockwise();
            Invoke();
            void RotateQuarterClockwise()
            {
                var tempDetails = new FalseMatrix<bool>(Height, Width, counter: new BoolCounter());
                
                for(int y = 0; y < Width; y++)
                for (int x = 0; x < Height; x++)
                    tempDetails[Height - 1 - x, y] = this[y, x];
                this.details = tempDetails;

                bounds = new Box()
                {
                    X = X,
                    Y = Y,
                    Width = Height,
                    Height = Width,
                };
            }
        }

        public void HorizontalFlip()
        {
            var tempDetails = new FalseMatrix<bool>(Width, Height, counter: new BoolCounter());

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                tempDetails[x, y] = this[Width - 1 - x, y];
            this.details = tempDetails;
            Invoke();
        }

        public void VerticalFlip()
        {
            var tempDetails = new FalseMatrix<bool>(Width, Height, counter: new BoolCounter());

            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                tempDetails[x, Height - 1 - y] = this[x, y];
            this.details = tempDetails;
            Invoke();
        }

        public void Scale(int width, int height)
        {
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            
            var tempDetails = new FalseMatrix<bool>(width, height, counter: new BoolCounter());

            (int minWidth, int minHeight) = (Width, Height);
            if (width < Width) minWidth = width;
            if (height < Height) minHeight = height;
            
            for(int x  = 0; x < minWidth; x++)
            for (int y = 0; y < minHeight; y++)
                tempDetails[x, y] = this[x, y];
            this.details = tempDetails;
            
            bounds = new Box()
            {
                X = bounds.X,
                Y = bounds.Y,
                Width = width,
                Height = height
            };
            
            Invoke();
        }
        
        #endregion

        public static Shape DefaultShape()
        {
            Box defaultBox = new Box()
            {
                X = 0,
                Y = 0,
                Width = 2,
                Height = 2,
            };
            return new Shape(defaultBox);
        }

        public override string ToString()
        {
            return this.ToCypherString();
        }
    }
}