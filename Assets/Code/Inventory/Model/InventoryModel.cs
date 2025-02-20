using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Inventory
{
    public class InventoryModel
    {
        public ObservableVolumetricMatrix<Item> Items;
        public InventoryData Data = new InventoryData();

        public Item this[int x, int y] => Items[x, y];
        public Item this[int index] => Items[index];
        
        public event Action OnModelChanged
        {
            add => Items.AnyValueChanged += value;
            remove => Items.AnyValueChanged -= value;
        }
        
        public InventoryModel(IEnumerable<Item> items, Shape shape)
        {
            Items = new ObservableVolumetricMatrix<Item>(shape, items.ToList());
        }

        public void Bind(InventoryData Data)
        {
            this.Data = Data;

            bool isNew = Data.Items == null || Data.Items.UniqueItems == null || Data.Items.UniqueItems.Count == 0;
            
            if (isNew) Data.FillData(Items.GetCore());
            else Items.ReplaceCore(Data.Items);
        }
        
        public void Clear() => Items.Clear();
        public bool Add(Item item) => Items.TryAdd(item);
        public bool Remove(Item item) => Items.TryRemove(item);

        public bool TryCombine(Item item1, Item item2)
        {
            if (item1.detailsId != item2.detailsId) return false;
            
            int total = item1.quantity + item2.quantity;
            int maxStack = item1.details.MaxStack;
            int quantity1 = total > maxStack ? maxStack : total;
            int quantity2 = total - quantity1;
            
            item2.quantity = quantity1;

            if(quantity2 > 0)
            {
                item1.quantity = quantity2;
            }
            else
            {
                Remove(item1);
            }
            
            Items.Invoke();
            
            return true;
        }
    }
}