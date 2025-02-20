namespace Systems
{
    public interface ICounter<T>
    {
        bool Count(T item);
    }
    
    public class NullCounter<T> : ICounter<T>
    {
        public bool Count(T item) => item != null;
    }

    public class BoolCounter : ICounter<bool>
    {
        public bool Count(bool item) => item;
    }
    
    public class IntNonZeroCounter : ICounter<int>
    {
        public bool Count(int item) => item != 0;
    }

    public static class Counter<T>
    {
        public static ICounter<T> Null = new NullCounter<T>();
    }
}