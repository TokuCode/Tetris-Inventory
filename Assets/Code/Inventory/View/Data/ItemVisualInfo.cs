using UnityEngine;

namespace Systems.Inventory
{
    public class ItemVisualInfo : IData<Item>
    {
        public Box bounds;
        public Sprite Icon;
        public Sprite IconRotation;
        public Sprite IconRotation180;
        public Sprite IconRotation270;
        public Sprite IconFlip;
        public Sprite IconRotationFlip;
        public Sprite IconRotation180Flip;
        public Sprite IconRotation270Flip;
        public Item Item;

        public ItemVisualInfo(Item item = null)
        {
            SetData(item);
        }
        
        public void SetData(Item item)
        {
            Item = item;
            if (item == null) return;

            Icon = item.details.Icon;
            IconRotation = item.details.IconRotation;
            IconRotation180 = item.details.IconRotation180;
            IconRotation270 = item.details.IconRotation270;
            IconFlip = item.details.IconFlip;
            IconRotationFlip = item.details.IconRotationFlip;
            IconRotation180Flip = item.details.IconRotation180Flip;
            IconRotation270Flip = item.details.IconRotation270Flip;
            
            bounds = new Box()
            {
                X = item.Shape.X,
                Y = item.Shape.Y,
                Width = item.Shape.Width,
                Height = item.Shape.Height
            };
        }

        public Item GetData() => Item;
    }
}