using System;
using Unity.Properties;

namespace Systems 
{
    public class BindableProperty<T>
    {
        readonly Func<T> getter;
        readonly Action<T> setter;

        public BindableProperty(Func<T> getter, Action<T> setter = null)
        {
            this.getter = getter;
            this.setter = setter;
        }
        
        [CreateProperty]
        public T Value
        {
            get => getter();
            set => setter(value);
        }
        
        public static BindableProperty<T> Bind(Func<T> getter, Action<T> setter = null) => new BindableProperty<T>(getter, setter);
    }
}