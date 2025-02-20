using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public class InventoryView : StorageView
    {
        public override IEnumerator InitializeView(ViewModel viewModel)
        {
            root = document.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(styleSheet);
            container = root.CreateChild("container");
            
            slots = new Slot[viewModel.Shape.Area];

            var slotsProperties = new BindablePropertyArray<Item>(i => viewModel.Items[i]);

            var inventory = container.CreateChild("inventory").WithManipulator(new PanelDragManipulator());
            inventory.style.width = InventoryViewProperties.Length(viewModel.Shape.Width);
            inventory.style.height = InventoryViewProperties.Length(viewModel.Shape.Height);
            
            var slotContainer = inventory.CreateChild("slotContainer");
            for (int i = 0; i < viewModel.Shape.Area; i++)
            {
                var slot = slotContainer.CreateChild<Slot>("slot");
                slot.ItemProperty = slotsProperties.GetProperty(i);
                slot.SetLockedState(viewModel.Shape[i]);
                RegisterOnPointerDownInSlot(slot);
                slots[i] = slot;
            }

            itemViewPool = new ItemViewPool(inventory, InventoryViewProperties.ItemViewPoolSize);
            
            ghostIcon = container.CreateChild<GhostIcon>("ghostIcon");

            yield return null;
        }
    }
}