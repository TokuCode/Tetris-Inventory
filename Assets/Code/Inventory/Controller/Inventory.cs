using System;
using System.Collections.Generic;
using Systems.Persistence;
using UnityEngine;

namespace Systems.Inventory
{
    public class Inventory : MonoBehaviour, IShapeLoader, IBind<InventoryData>
    {
        [SerializeField] private InventoryView view;
        [SerializeField] private Shape shape;
        [SerializeField] private List<ItemInputData> items;
        [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        
        private InventoryController controller;

        private void Awake()
        {
            var startingItems = new List<Item>();
            foreach (var itemData in items)
            {
                Item item = itemData.ItemDetails.Create(itemData.Quantity);
                item.Shape.Move(itemData.X, itemData.Y, false);
                startingItems.Add(item);
            }
            
            controller = new InventoryController.Builder(view)
                .WithStartingItems(startingItems)
                .WithShape(shape)
                .Build();
        }
        
        public void Bind(InventoryData data)
        {
            controller.Bind(data);
            data.Id = Id;
        }
        
        public void SetShape(Shape shape)
        {
            this.shape = shape;
        }
        
        public Shape GetShape()
        {
            return shape;
        }
    }
}