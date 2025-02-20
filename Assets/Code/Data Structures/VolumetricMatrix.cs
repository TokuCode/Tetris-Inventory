using System;
using UnityEngine;
using System.Collections.Generic;
using Systems.Inventory;
using Unity.VisualScripting;

namespace Systems
{
    [Serializable]
    public class VolumetricMatrix<T> : IVolumetricMatrix<T>, ICloneable where T : IVolumetricItem, ICloneable
    {
        [SerializeField, HideInInspector] private FalseMatrix<int> space;
        public Shape shape { get; private set; }
        
        [field: SerializeField] public List<T> UniqueItems { get; set; }
        
        public int X => shape.X;
        public int Y => shape.Y;
        public int Width => shape.Width;
        public int Height => shape.Height;
        public int TotalArea => shape.Area;
        public int Area => shape.FreeSpace;
        public int FreeArea => Area - space.Count;
        public int Count => UniqueItems.Count;
        
        public VolumetricMatrix(Shape shape, IList<T> initialList = null)
        {
            this.shape = shape;
            space = new FalseMatrix<int>(shape.Width, shape.Height, counter: new IntNonZeroCounter());
            UniqueItems = new List<T>();
            if (initialList == null) return;
            
            foreach (var volumetricItem in initialList)
                TryAdd(volumetricItem);
        }

        #region Positioning

        int GetIndex(int x, int y) => y * Width + x;
        
        void GetPosition(int index, out int x, out int y)
        {
            x = index % Width;
            y = index / Width;
        }
        
        T GetItem(int index) => space[index] > 0 ? UniqueItems[space[index] - 1] : default;
        void SetItem(int index, T item) => space[index] = UniqueItems.IndexOf(item) + 1;

        public T this[int x, int y]
        {
            get => GetItem(GetIndex(x, y));
            private set => SetItem(GetIndex(x, y), value);
        } 
        
        public T this[int index]
        {
            get => GetItem(index);
            private set => SetItem(index, value);
        }

        private void SetItem(T item)
        {
            for(int x = 0; x < item.Shape.Width; x++)
            for (int y = 0; y < item.Shape.Height; y++)
            {
                if (!item.Shape[x, y]) continue;
                
                var (relativeX, relativeY) = (item.Shape.X + x - shape.X, item.Shape.Y + y - shape.Y);
                this[relativeX, relativeY] = item;
            }
        }

        private void ClearSpace()
        {
            for (int x = 0; x < Width; x++)
            for(int y = 0; y < Height; y++) 
                space[x, y] = 0;
        }

        private void Normalize()
        {
            ClearSpace();
            UniqueItems.ForEach(SetItem);
        }

        #endregion

        #region Operations

        public bool TryAdd(T item)
        {
            if (UniqueItems.Contains(item)) return true;
            
            var shapes = new List<Shape>();
            UniqueItems.ForEach(item => shapes.Add(item.Shape));

            if (!item.Shape.FitInsideOf(shape, shapes.ToArray())) return false;
            
            UniqueItems.Add(item);
            SetItem(item);

            return true;
        }

        public bool TryRemove(T item)
        {
            if(!UniqueItems.Contains(item)) return false;
            
            UniqueItems.Remove(item);
            Normalize();
            
            return true;
        }

        public void Clear()
        {
            ClearSpace();
            UniqueItems.Clear();
        }
        
        #endregion

        public object Clone()
        {
            List<T> cloneItems = new List<T>();
            foreach (var item in UniqueItems)
            {
                cloneItems.Add((T) item.Clone());
            }

            var clone = new VolumetricMatrix<T>(shape.Clone() as Shape, cloneItems);
            return clone;
        }
    }
}