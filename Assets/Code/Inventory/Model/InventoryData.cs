using System;
using UnityEngine;

namespace Systems.Inventory 
{
    [Serializable]
    public class InventoryData : ISaveable
    {
        [field: SerializeField] public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        public VolumetricMatrix<Item> Items;

        public void FillData(VolumetricMatrix<Item> Items)
        {
            this.Items = Items;
        }
    }
}