using UnityEngine;

namespace OutfoxeedTools
{
    public class WorldLoaderMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private WorldConfig _worldConfig;

        public void LoadWorld() => WorldLoader.LoadWorld(_worldConfig);
        public void ReloadWorld() => WorldLoader.ReloadWorld();
    }
}