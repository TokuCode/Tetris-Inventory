using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Systems.Inventory
{
    [Serializable]
    public class Item : IVolumetricItem, ICloneable
    {
        public SerializableGuid id;
        public SerializableGuid detailsId;
        public ItemDetails details;
        public int quantity;
        [field: SerializeField] public Shape Shape { get; set; }
        [HideInInspector] public ShapeIsometryGroup transformations;
        
        public Item(ItemDetails details, int quantity)
        {
            id = SerializableGuid.NewGuid();
            detailsId = details.Id;
            this.details = details;
            this.quantity = quantity;
            Shape = details.Shape.Clone() as Shape;
            transformations = new ShapeIsometryGroup(0, 0);
        }

        #region ShapeTransformations

        public void RotateClockwise()
        {
            Shape.Rotate(1);
            transformations.RotateClockwise();
        }
        
        public void RotateCounterClockwise()
        {
            Shape.Rotate(1, false);
            transformations.RotateCounterClockwise();
        }

        public void HorizontalFlip()
        {
            Shape.HorizontalFlip();
            transformations.ReflectHorizontally();
        }
        
        public void VerticalFlip()
        {
            Shape.VerticalFlip();
            transformations.ReflectVertically();
        }

        #endregion

        public object Clone()
        {
            var clone = new Item(details, quantity);
            Item cloneAsItem = clone as Item;
            cloneAsItem.transformations = new ShapeIsometryGroup(transformations.rotation, transformations.reflection);

            return clone;
        }
    }
}