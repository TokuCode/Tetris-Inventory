namespace Systems
{
    public interface IPooleable<T>
    {
        bool IsActive { get; }
        void Set(IData<T> data);
        void Activate(bool active);
        void Reset();
    }
    
    public interface IData<T>
    {
        void SetData(T data);
        T GetData();
    }
}