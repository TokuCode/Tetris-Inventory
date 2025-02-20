using UnityEngine.UIElements;
using System;
using UnityEngine;

namespace Systems.Inventory
{
    public class ItemView : VisualElement, IPooleable<Item>, IStylish
    {
        private bool Active;
        public bool IsActive => Active;
        
        public ItemVisualInfo Data { get; private set; }
        
        public VisualElement Icon;
        public Label Quantity;

        public ItemView()
        {
            Data = new ItemVisualInfo();
            Icon = this.CreateChild("itemIcon");
            Quantity = Icon.CreateChild<Label>("stackCount");
            Icon.pickingMode = PickingMode.Ignore;
            Quantity.pickingMode = PickingMode.Ignore;
            pickingMode = PickingMode.Ignore;
        }
        
        public void Set(IData<Item> data)
        {
            Data.SetData(data.GetData());
            SetStyle();
        }
        
        public void Activate(bool active)
        {
            Active = active;
            SetStyle();
        }

        public void Reset()
        {
            Data = new ItemVisualInfo();
        }

        public void SetStyle()
        {
            style.position = Position.Absolute;
            style.left = Data.bounds.X * InventoryViewProperties.CellSize;
            style.top = Data.bounds.Y * InventoryViewProperties.CellSize;
            style.width = Data.bounds.Width * InventoryViewProperties.CellSize;
            style.height = Data.bounds.Height  * InventoryViewProperties.CellSize;
            
            Quantity.style.color = Color.black;
            Quantity.style.position = Position.Absolute;
            Quantity.style.right = 0f;
            Quantity.style.bottom = 0f;
            Quantity.style.fontSize = 12f;
            
            if (Data.Item != null)
            {
                Icon.style.width = Data.bounds.Width * InventoryViewProperties.CellSize;
                Icon.style.height = Data.bounds.Height * InventoryViewProperties.CellSize;
                Icon.style.backgroundImage = Data.Icon.texture;
                
                TransformImageWithItem();
                
                Quantity.text = Data.Item.quantity > 1 ? Data.Item.quantity.ToString() : string.Empty;
            }
            else
            {
                Icon.style.width = 0f;
                Icon.style.height = 0f;
                Icon.style.backgroundImage = null;
                
                Quantity.text = string.Empty;
            }
        }

        private void TransformImageWithItem()
        {
            if (Data.Item == null) return;
            
            int rotations = Data.Item.transformations.rotation;
            bool reflection = Data.Item.transformations.reflection == 1;

            switch (rotations, reflection)
            {
                case (0, false):
                    Icon.style.backgroundImage = Data.Icon.texture;
                    break;
                
                case (1, false):
                    Icon.style.backgroundImage = Data.IconRotation.texture;
                    break;
                
                case (2, false):
                    Icon.style.backgroundImage = Data.IconRotation180.texture;
                    break;
                
                case (3, false):
                    Icon.style.backgroundImage = Data.IconRotation270.texture;
                    break;
                
                case (0, true):
                    Icon.style.backgroundImage = Data.IconFlip.texture;
                    break;
                
                case (1, true):
                    Icon.style.backgroundImage = Data.IconRotationFlip.texture;
                    break;
                
                case (2, true):
                    Icon.style.backgroundImage = Data.IconRotation180Flip.texture;
                    break;
                
                case (3, true):
                    Icon.style.backgroundImage = Data.IconRotation270Flip.texture;
                    break;
            }
        }
    }
}