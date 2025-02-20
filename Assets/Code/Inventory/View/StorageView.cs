using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace Systems.Inventory 
{
    public abstract class StorageView : MonoBehaviour
    {
        protected Slot[] slots;
        protected ItemViewPool itemViewPool;
        public ItemViewPool ItemViewPool => itemViewPool;
        public GhostIcon ghostIcon;
        
        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;

        private bool isDragging;

        protected VisualElement root;
        protected VisualElement container;

        public event Action<Item, IList<Slot>> OnDrop;
        public event Action<Item> OnStartDrag;
        public event Action<Item> OnFailedDrop;

        private void Start()
        {
            ghostIcon.RegisterCallback<PointerDownEvent>(OnPointerDown);
            ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        public abstract IEnumerator InitializeView(ViewModel viewModel);

        protected void RegisterOnPointerDownInSlot(Slot slot)
        {
            slot.OnStartDrag += OnPointerDown;
        }

        void OnPointerDown(Vector2 position, Slot slot)
        {
            isDragging = true;

            var item = slot.ItemInSlot;
            
            if(item == null) return;
           
            ghostIcon.ShowGhostIcon(item);
            
            ghostIcon.ShowGhostIcon(item);
            ghostIcon.SetGhostIconPosition(position);
            
            OnStartDrag.Invoke(item);
        }

        void OnPointerDown(PointerDownEvent evt)
        {
            if (!isDragging || evt.button == 0) return;
            
            ghostIcon.ghostItem.RotateClockwise();
            ghostIcon.SetGhostIconPosition(evt.position);
            ghostIcon.SetStyle();
        }
        
        void OnPointerMove(PointerMoveEvent evt)
        {
            if (!isDragging) return;
            
            ghostIcon.SetGhostIconPosition(evt.position);
        }
        
        void OnPointerUp(PointerUpEvent evt)
        {
            if (!isDragging || evt.button != 0) return;

            var overlapSlots = slots
                .Where(slot => slot.worldBound.Overlaps(ghostIcon.worldBound))
                .OrderBy(slot => Vector2.Distance(slot.worldBound.position, ghostIcon.worldBound.position))
                .ToList();

            if (overlapSlots.Count > 0)
                OnDrop?.Invoke(ghostIcon.ghostItem, overlapSlots);
            else
                OnFailedDrop?.Invoke(ghostIcon.ghostItem);
            
            isDragging = false;
            ghostIcon.HideGhostIcon();
        }

        public void Reset(ViewModel viewModel)
        {
            root.Clear();
            StartCoroutine(InitializeView(viewModel));
            
            ghostIcon.RegisterCallback<PointerDownEvent>(OnPointerDown);
            ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            ItemViewPool.RequestRange(viewModel.Items.UniqueItems.ToArray());
        }
    }
}