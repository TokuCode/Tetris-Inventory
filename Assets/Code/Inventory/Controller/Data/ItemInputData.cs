using System;
using UnityEngine;

namespace Systems.Inventory
{
    [Serializable]
    public struct ItemInputData
    {
        public ItemDetails ItemDetails;
        public int Quantity;
        public int X, Y;
    }
}