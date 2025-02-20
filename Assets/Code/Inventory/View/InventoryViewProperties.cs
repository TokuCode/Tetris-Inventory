using UnityEngine;

namespace Systems.Inventory
{
    public class InventoryViewProperties : Singleton<InventoryViewProperties>
    {
        public const int CellSize = 64;
        public const int Margin = 1;
        public const int ItemViewPoolSize = 10;

        public static int Length(int length) => length * (CellSize + Margin);

        public Sprite FreeSlotIcon;
        public Sprite LockedSlotIcon;
    }
}