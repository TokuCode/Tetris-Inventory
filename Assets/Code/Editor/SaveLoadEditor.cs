using UnityEditor;
using UnityEngine;

namespace Systems.Persistence.Editor
{
    [CustomEditor(typeof(SaveLoadSystem))]
    public class SaveLoadEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaveLoadSystem saveLoadSystem = target as SaveLoadSystem;
            string gameName = saveLoadSystem.gameData.Name;
            
            DrawDefaultInspector();
            
            if (GUILayout.Button("New Game"))
            {
                saveLoadSystem.NewGame();
            }
            if (GUILayout.Button("Save Game"))
            {
                saveLoadSystem.SaveGame();
            }
            if (GUILayout.Button("Load Game"))
            {
                saveLoadSystem.LoadGame(gameName);
            }
            if (GUILayout.Button("Delete Game"))
            {
                saveLoadSystem.DeleteGame(gameName);
            }

        }
    }
}