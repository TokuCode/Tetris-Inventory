using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public class Slot : VisualElement, IStylish
    {
        public int Index => parent.IndexOf(this);
        public BindableProperty<Item> ItemProperty;
        public Item ItemInSlot => ItemProperty.Value;
        private bool isLocked;
        
        public Action<Vector2, Slot> OnStartDrag = delegate { };

        public Slot()
        {
            SetStyle();
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }
        
        void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0) return;
            
            OnStartDrag.Invoke(evt.position, this);
            evt.StopPropagation();
        }

        public void SetLockedState(bool isLocked)
        {
            this.isLocked = isLocked;
            SetStyle();
        }

        public void SetStyle()
        {
            style.width = InventoryViewProperties.CellSize;
            style.height = InventoryViewProperties.CellSize;
            style.marginRight = InventoryViewProperties.Margin;
            style.marginBottom = InventoryViewProperties.Margin;
            SetIcon();
        }

        private void SetIcon()
        {
            style.backgroundImage = isLocked ? InventoryViewProperties.Instance.LockedSlotIcon.texture : InventoryViewProperties.Instance.FreeSlotIcon.texture;
        }
    }
}