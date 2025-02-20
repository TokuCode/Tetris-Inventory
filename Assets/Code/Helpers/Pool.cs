using System.Collections.Generic;

namespace Systems
{
    public abstract class Pool<T, TData> where T : IPooleable<TData>
    {
        protected List<T> pool;
        protected int size;
        
        public Pool(int size)
        { 
            this.size = size;
        }
        
        protected void Initialize()
        {
            pool = new List<T>();
            
            for(int i = 0; i < size; i++)
            {
                T instance = Instantiate();
                instance.Activate(false);
                pool.Add(instance);
            }
        }

        public abstract T Instantiate();

        public T Request(IData<TData> data)
        {
            for(int i = 0; i < size; i++)
            {
                if (!pool[i].IsActive)
                {
                    pool[i].Set(data);
                    pool[i].Activate(true);
                    return pool[i];
                }
            }

            var expanded = ExpandPool();
            expanded.Set(data);
            expanded.Activate(true);
            return expanded;
        }

        public T ExpandPool()
        {
            T instace = Instantiate();
            pool.Add(instace);
            size++;

            return instace;
        }
        
        public void Reset(T instance)
        {
            instance.Activate(false);
            instance.Reset();
        }
    }
}