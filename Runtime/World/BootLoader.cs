using UnityEngine;

namespace OutfoxeedTools
{
    public class BootLoader : MonoBehaviour
    {
        [SerializeField] private WorldConfig _bootWorldConfig;
        
        private void Awake()
        {
            WorldLoader.LoadWorld(_bootWorldConfig);
        }
    }
}