using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    [Serializable]
    public class ObservableVolumetricMatrix<T> : IObservableVolumetricMatrix<T> where T : IVolumetricItem, ICloneable
    {
        [SerializeField, HideInInspector] private VolumetricMatrix<T> core;
        public Shape shape => core.shape;

        public event Action AnyValueChanged;
        public void Invoke() => AnyValueChanged?.Invoke();
        
        public int Width => core.Width;
        public int Height => core.Height;
        public int TotalArea => core.TotalArea;
        public int Area => core.Area;
        public int FreeArea => core.FreeArea;
        public int Count => core.Count;
        public List<T> UniqueItems => core.UniqueItems;
        public T this[int x, int y] => core[x, y];
        public T this[int index] => core[index];

        public ObservableVolumetricMatrix(Shape shape, IList<T> initialList = null)
        {
            core = new VolumetricMatrix<T>(shape, initialList);
        }

        #region Operations
        
        public bool TryAdd(T item)
        {
            if(!core.TryAdd(item)) return false;
            Invoke();
            return true;
        }
        
        public bool TryRemove(T item)
        {
            if(!core.TryRemove(item)) return false;
            Invoke();
            return true;
        }
        
        public void Clear()
        {
            core.Clear();
            Invoke();
        }

        #endregion

        public VolumetricMatrix<T> GetCore()
        {
            return core;
        }
        
        public void ReplaceCore(VolumetricMatrix<T> newCore)
        {
            core = newCore;
        }
    }
}