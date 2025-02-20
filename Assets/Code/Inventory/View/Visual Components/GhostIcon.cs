using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public class GhostIcon : VisualElement, IStylish
    {
        public Item ghostItem { get; private set; }
        public Shape originalShape { get; private set; }
        public ShapeIsometryGroup originalIsometry { get; private set; }
        public bool isShown => ghostItem != null;
        private int x, y;
        private Image Icon;
        
        public GhostIcon()
        {
            Icon = this.CreateChild<Image>("ghostImage");
            SetStyle();
        }

        public void SetGhostIconPosition(Vector2 position)
        {
            x = (int)position.x;
            y = (int)position.y;
            SetStyle();
        }
        
        public void ShowGhostIcon(Item item)
        {
            ghostItem = item;
            originalShape = (Shape)item.Shape.Clone();
            originalIsometry = new ShapeIsometryGroup(item.transformations.rotation, item.transformations.reflection);
            SetStyle();
        }

        public void HideGhostIcon()
        {
            ghostItem = null;
            originalShape = null;
            originalIsometry = null;
            SetStyle();
        }

        public void SetStyle()
        {
            style.width = isShown ? ghostItem.Shape.Width * InventoryViewProperties.CellSize : 0f;
            style.height = isShown ? ghostItem.Shape.Height * InventoryViewProperties.CellSize : 0f;
            
            style.visibility = isShown ? Visibility.Visible : Visibility.Hidden;
            
            TransformOperations();
            Icon.style.width = isShown ? ghostItem.Shape.Width * InventoryViewProperties.CellSize : 0f;
            Icon.style.height = isShown ? ghostItem.Shape.Height * InventoryViewProperties.CellSize : 0f;
            
            style.position = Position.Absolute;
            style.left = x - layout.width / 2;
            style.top = y - layout.height / 2;
        }

        public void TransformOperations()
        {
            if (ghostItem == null) return;
            
            int rotations = ghostItem.transformations.rotation;
            bool reflection = ghostItem.transformations.reflection == 1;

            switch (rotations, reflection)
            {
                case (0, false):
                    Icon.style.backgroundImage = ghostItem.details.Icon.texture;
                    break;
                
                case (1, false):
                    Icon.style.backgroundImage = ghostItem.details.IconRotation.texture;
                    break;
                
                case (2, false):
                    Icon.style.backgroundImage = ghostItem.details.IconRotation180.texture;
                    break;
                
                case (3, false):
                    Icon.style.backgroundImage = ghostItem.details.IconRotation270.texture;
                    break;
                
                case (0, true):
                    Icon.style.backgroundImage = ghostItem.details.IconFlip.texture;
                    break;
                
                case (1, true):
                    Icon.style.backgroundImage = ghostItem.details.IconRotationFlip.texture;
                    break;
                
                case (2, true):
                    Icon.style.backgroundImage = ghostItem.details.IconRotation180Flip.texture;
                    break;
                
                case (3, true):
                    Icon.style.backgroundImage = ghostItem.details.IconRotation270Flip.texture;
                    break;
            }
        }
    }
}