using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Systems.Inventory
{
    public class InventoryController
    {
        private readonly InventoryView view;
        private readonly InventoryModel model;
        private readonly Shape shape;
        
        public InventoryController(InventoryView view, InventoryModel model, Shape shape)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(shape != null, "Shape is null");
            this.view = view;
            this.model = model;
            this.shape = shape;
            
            view.StartCoroutine(Initialize());
        }
        
        public void Bind(InventoryData data) => model.Bind(data);

        IEnumerator Initialize()
        {
            yield return view.InitializeView(new ViewModel(shape, model.Items.GetCore()));
            
            view.OnDrop += HandleDrop;
            
            view.OnStartDrag += HandleStartDrag;
            view.OnFailedDrop += HandleFailedDrop;
            
            model.OnModelChanged += HandleOnModelChanged;
            
            RefreshView();
        }

        void HandleDrop(Item ghostItem, IList<Slot> overlapSlots)
        {
            foreach (var slot in overlapSlots)
            {
                if(slot == null) continue;
                
                int x = slot.Index % shape.Width;
                int y = slot.Index / shape.Width;
                
                ghostItem.Shape.Move(x, y, false);
                if(model.Add(ghostItem)) return;
                if(Combine(ghostItem)) return;
            }
            
            HandleFailedDrop(ghostItem);
        }
        
        void HandleFailedDrop(Item ghostItem)
        {
            ghostItem.Shape = view.ghostIcon.originalShape;
            ghostItem.transformations = view.ghostIcon.originalIsometry;
            model.Add(ghostItem);
        }

        void HandleStartDrag(Item item)
        {
            model.Remove(item);
        }

        bool Combine(Item ghostItem)
        {
            for(int x = 0; x < ghostItem.Shape.Width; x++)
            for (int y = 0; y < ghostItem.Shape.Height; y++)
            {
                int X = x + ghostItem.Shape.X - shape.X;
                int Y = y + ghostItem.Shape.Y - shape.Y;
                
                if(X < 0 || X >= shape.Width || Y < 0 || Y >= shape.Height) continue;
                
                var item = model.Items[X, Y];
                
                if(item == null) continue;

                if (model.TryCombine(ghostItem, item))
                {
                    return true;
                }
            }
            return false;
        }

        void RefreshView()
        {
            view.ItemViewPool.Reset();
            view.ItemViewPool.RequestRange(model.Items.UniqueItems.ToArray());
        }

        void HandleOnModelChanged() => RefreshView();

        #region Builder
        
        public class Builder
        {
            private InventoryView view;
            private IEnumerable<Item> startingItems;
            private Shape shape;
            
            public Builder(InventoryView view)
            {
                this.view = view;
            }
            
            public Builder WithStartingItems(IEnumerable<Item> startingItems)
            {
                this.startingItems = startingItems;
                return this;
            }
            
            public Builder WithShape(Shape shape)
            {
                this.shape = shape;
                return this;
            }
            
            public InventoryController Build()
            {
                InventoryModel model = startingItems != null
                    ? new InventoryModel(startingItems, shape)
                    : new InventoryModel(Array.Empty<Item>(), shape);
                
                return new InventoryController(view, model, shape);
            }
        }

        #endregion
    }
}