using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Systems 
{
    [Serializable]
    public class FalseMatrix<T> : ICloneable
    {
        [field: SerializeField] public T[] array { get; private set; }
        
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
         
        public int Area => Width * Height;
        public int Count => array.Count(i => i != null);

        public T this[int x, int y]
        {
            get
            {
                if (!IsInside(x, y)) return default;
                return array[GetIndex(x, y)];
            }
            set
            {
                if (!IsInside(x, y)) return;
                array[GetIndex(x, y)] = value;
            }
        }

        private ICounter<T> counter;
        
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Area) return default;
                return array[index];
            }
            set
            {
                if (index < 0 || index >= Area) return;
                array[index] = value;
            } 
        }

        public FalseMatrix(int width, int height, IList<T> initialValues = null, ICounter<T> counter = null)
        {
            Width = width;
            Height = height;
            array = new T[Area];
            if (initialValues != null)
            {
                initialValues.Take(Area).ToArray().CopyTo(array, 0);
            }

            if (counter == null)
            {
                this.counter = Counter<T>.Null;
                return;
            }
            
            this.counter = counter;
        }

        private int GetIndex(int x, int y)
        { 
            if (!IsInside(x, y)) return -1;
            return y * Width + x;
        } 
        private bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
        
        public object Clone()
        {
            var clone = new FalseMatrix<T>(Width, Height, array, counter);
            return clone;
        }
    }
}