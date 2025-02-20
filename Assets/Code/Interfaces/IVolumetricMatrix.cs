using System.Collections.Generic;

namespace Systems
{
    public interface IVolumetricMatrix<T> where T : IVolumetricItem
    {
        T this[int x, int y] { get; }
        
        int Width { get; }
        int Height { get; }
        int Area { get; }
        int Count { get; }
       
        List<T> UniqueItems { get; }

        bool TryAdd(T item);
        bool TryRemove(T item);
        void Clear();
    }
}