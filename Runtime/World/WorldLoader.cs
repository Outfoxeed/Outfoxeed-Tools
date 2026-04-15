using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace OutfoxeedTools
{
    public static class WorldLoader
    {
        public static WorldConfig LoadedWorldConfig { get; private set; }
        
        public static void LoadWorld(WorldConfig worldConfig)
        {
            if (!worldConfig || !worldConfig.IsValid())
            {
                Debug.LogError("Invalid World Config");
                return;
            }

            Time.timeScale = 1f;
            
            bool additive = false;
            foreach (WorldConfig dependencyWorld in worldConfig.WorldDependencies)
            {
                LoadWorld(dependencyWorld, additive);
                additive = true;
            }
            
            LoadWorld(worldConfig, additive);
            LoadedWorldConfig = worldConfig;
            EventBus<RequestedWorldLoadedEvent>.Raise(new RequestedWorldLoadedEvent(worldConfig));
        }
        
        public static void ReloadWorld()
        {
#if UNITY_EDITOR
            if (!LoadedWorldConfig)
            {
                EditorReloadNonSelectedWorld();
            }
            else
#endif
            {
                LoadWorld(LoadedWorldConfig);
            }
        }

        
        private static void LoadWorld(WorldConfig worldConfig, bool additive)
        {
            if (!worldConfig.IsValid())
            {
                Debug.LogError($"Config with name '{worldConfig.name}' is invalid");
                return;
            }

            Scene sceneByName = SceneManager.GetSceneByName(worldConfig.MainScene);
            if (sceneByName.IsValid())
            {
                Debug.Log($"World loading request {worldConfig.name} ignored because its main scene is already loaded");
                return;
            }

            LoadScene(worldConfig.MainScene, additive);
            foreach (string subScene in worldConfig.SubScenes)
            {
                LoadScene(subScene, true);
            }
            
            EventBus<WorldLoadedEvents>.Raise(new WorldLoadedEvents(worldConfig));
        }

        private static void LoadScene(string scene, bool additive)
        {
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorSceneManager.OpenScene(scene, additive ? OpenSceneMode.Additive : OpenSceneMode.Single);
            }
            else
#endif
            {
                SceneManager.LoadScene(scene, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            }
        }
        
#if UNITY_EDITOR
        private static void EditorReloadNonSelectedWorld()
        {
            string[] loadedScenes = new string[SceneManager.loadedSceneCount];
            for (int i = 0; i < SceneManager.loadedSceneCount; i++)
                loadedScenes[i] = SceneManager.GetSceneAt(i).path;
            
            foreach (GUID worldConfigAssetGUID in AssetDatabase.FindAssetGUIDs($"t:{nameof(WorldConfig)}"))
            {
                WorldConfig worldConfig = AssetDatabase.LoadAssetAtPath<WorldConfig>(AssetDatabase.GUIDToAssetPath(worldConfigAssetGUID));
                IReadOnlyList<string> worldScenes = GetWorldScenes(worldConfig);

                if (worldScenes.SequenceEqual(loadedScenes))
                {
                    LoadWorld(worldConfig);
                    return;
                }
            }
            IReadOnlyList<string> GetWorldScenes(WorldConfig worldConfig)
            {
                List<string> worldScenes = new List<string>(2);
                foreach (WorldConfig dependency in worldConfig.WorldDependencies)
                    worldScenes.AddRange(GetWorldScenes(dependency));
                
                worldScenes.Add(worldConfig.MainScene);
                foreach (string subScene in worldConfig.SubScenes)
                    worldScenes.Add(subScene);
                return worldScenes;
            }
        }
#endif
    }
}