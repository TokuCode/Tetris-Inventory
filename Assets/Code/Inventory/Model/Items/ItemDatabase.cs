using System.Collections.Generic;
using UnityEngine;

namespace Systems.Inventory
{
    public static class ItemDatabase
    {
        private static Dictionary<SerializableGuid, ItemDetails> itemsDB;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Initialize()
        {
            itemsDB = new Dictionary<SerializableGuid, ItemDetails>();
            
            var itemDetails = Resources.LoadAll<ItemDetails>("");
            
            foreach (var itemDetail in itemDetails)
            {
                itemsDB.Add(itemDetail.Id, itemDetail);
            }
        }
        
        public static ItemDetails GetItemDetailsById(SerializableGuid id)
        {
            try
            {
                return itemsDB[id];
            }
            catch
            {
                Debug.LogError($"Item details not found for id: {id}");
                return null;
            }
        }
    }
}