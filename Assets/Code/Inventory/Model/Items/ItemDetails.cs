using UnityEngine;

namespace Systems.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemDetails : ScriptableObject, IShapeLoader
    {
        public string Name;
        public int MaxStack;
        
        public Sprite Icon;
        public Sprite IconRotation;
        public Sprite IconRotation180;
        public Sprite IconRotation270;
        public Sprite IconFlip;
        public Sprite IconRotationFlip;
        public Sprite IconRotation180Flip;
        public Sprite IconRotation270Flip;
        
        public string Description;
        public Shape Shape;
        public SerializableGuid Id = SerializableGuid.NewGuid();

        public void AssignNewId() => Id = SerializableGuid.NewGuid();

        public Item Create(int quantity)
        {
            return new Item(this, quantity);
        }
        
        public void SetShape(Shape shape) => Shape = shape;
        public Shape GetShape() => Shape;
    }
}