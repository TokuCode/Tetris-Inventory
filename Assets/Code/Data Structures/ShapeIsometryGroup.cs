using System;
using UnityEngine;

namespace Systems
{
    [Serializable]
    public class ShapeIsometryGroup
    {
        [field: SerializeField] public int rotation { get; private set; }
        [field: SerializeField] public int reflection { get; private set; }
        
        public ShapeIsometryGroup(int rotation, int reflection)
        {
            this.rotation = rotation;
            this.reflection = reflection;
        }

        private void VerticalReflectionRule()
        {
            if(reflection >= 2 && rotation >= 2) (reflection, rotation) = (reflection - 2, rotation - 2);
        }
        private void ReflectionRule() => reflection = reflection % 2;
        private void RotationRule() => rotation = (rotation + 4) % 4;

        private void RulesInOrder()
        {
            VerticalReflectionRule();
            ReflectionRule();
            RotationRule();
        }

        public void RotateClockwise()
        {
            rotation++;
            RulesInOrder();
        }

        public void RotateCounterClockwise()
        {
            rotation--;
            RulesInOrder();
        }

        public void ReflectHorizontally()
        {
            reflection++;
            RulesInOrder();
        }

        public void ReflectVertically()
        {
            rotation++;
            reflection++;
            RulesInOrder();
        }
    }
}