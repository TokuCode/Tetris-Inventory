using System;
using Systems.Inventory;

namespace Systems.Persistence 
{
    [Serializable]
    public class GameData
    {
        public string Name;
        public InventoryData Inventory;
    }
}