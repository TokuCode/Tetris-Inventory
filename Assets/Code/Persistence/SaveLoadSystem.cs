using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Systems.Inventory;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace Systems.Persistence
{
    public class SaveLoadSystem : Singleton<SaveLoadSystem>
    {
        [SerializeField] public InventoryView view;
        [SerializeField] public GameData gameData;

        
        private IDataService dataService;

        protected override void Awake()
        {
            base.Awake();
            dataService = new FileDataService(new JsonSerializer());
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => BindData();

        #region Save Load Operations

        public void NewGame()
        {
            gameData = new GameData() { Name = "New Game" };
        }

        public void SaveGame()
        {
            dataService.Save(gameData);
        }

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);
            BindData();
            var viewModel = new ViewModel(gameData.Inventory.Items.shape, gameData.Inventory.Items);
            view.Reset(viewModel);
        }

        public void DeleteGame(string gameName)
        {
            dataService.Delete(gameName);
        }

        #endregion

        #region Data Binding

        void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null)
            {
                if (data == null)
                {
                    data = new TData() {Id = entity.Id};
                }
                entity.Bind(data);
            }
        }

        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(d => d.Id == entity.Id);
                if (data == null)
                {
                    data = new TData { Id = entity.Id };
                    datas.Add(data);
                }
                entity.Bind(data);
            }
        }
        
        private void BindData()
        {
            Bind<Inventory.Inventory, InventoryData>(gameData.Inventory);
        }

        #endregion
    }
}