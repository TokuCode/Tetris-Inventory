namespace Systems.Inventory
{
    public class ViewModel
    {
        public readonly Shape Shape;
        public readonly VolumetricMatrix<Item> Items;
        
        public ViewModel(Shape shape, VolumetricMatrix<Item> items)
        {
            Shape = shape;
            Items = items;
        }
    }
}