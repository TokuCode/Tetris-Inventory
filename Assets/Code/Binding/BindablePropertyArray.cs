using System;
using Unity.Properties;

namespace Systems
{
    public class BindablePropertyArray<T>
    {
        readonly Func<int, T> getter;
        readonly Action<int, T> setter;

        [CreateProperty]
        public T this[int index]
        {
            get => getter(index);
            set => setter(index, value);
        }
        
        public BindablePropertyArray(Func<int, T> getter, Action<int, T> setter = null)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public BindableProperty<T> GetProperty(int index)
        {
            return new BindableProperty<T>(() => getter(index), value => setter(index, value));
        }
        
        public static BindablePropertyArray<T> Bind(Func<int, T> getter, Action<int, T> setter = null) => new BindablePropertyArray<T>(getter, setter);
    }
}