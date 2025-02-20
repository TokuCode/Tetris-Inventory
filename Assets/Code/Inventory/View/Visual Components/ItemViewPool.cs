using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public class ItemViewPool : Pool<ItemView, Item>
    {
        public VisualElement container;
        
        public ItemViewPool(VisualElement container, int size) : base(size)
        {
            this.container = container;
            Initialize();
        }
        
        public override ItemView Instantiate()
        {
            var itemView = container.CreateChild<ItemView>("itemView");
            return itemView;
        }
        
        public void Reset()
        {
            foreach (var itemView in pool)
            {
                Reset(itemView);
                itemView.SetStyle();
            }
        }
        
        public void RequestRange(Item[] itemRange)
        {
            for (int i = 0; i < itemRange.Length; i++)
            {
                if (itemRange[i] == null) continue;
                var data = new ItemVisualInfo(itemRange[i]);
                Request(data);
            }
        }
    }
}