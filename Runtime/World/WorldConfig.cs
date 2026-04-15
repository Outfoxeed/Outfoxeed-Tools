using UnityEngine;

namespace OutfoxeedTools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/WorldConfig")]
    public class WorldConfig : ScriptableObject
    {
        [field: SerializeField, Scene] public string MainScene { get; private set; }
        [field: SerializeField, Scene] public string[] SubScenes { get; private set; }

        [field: SerializeField] public WorldConfig[] WorldDependencies { get; private set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(MainScene);
        }
    }
}