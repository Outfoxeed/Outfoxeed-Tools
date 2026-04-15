using System.Collections.Generic;
using OutfoxeedTools.Attributes;
using UnityEngine;

namespace OutfoxeedTools.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private UiInfo[] uiInfos;

        [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystem;
        public UnityEngine.EventSystems.EventSystem EventSystem
        {
            get
            {
                if (eventSystem == null)
                    eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
                return eventSystem;
            }
        }

        [SerializeField] private UiType startUiType;
        private Dictionary<UiType, UiMenu> uiPrefabs;

        public enum UiType
        {
            None,
            Game,
            MainMenu,
            Pause,
            Options,
            LooseScreen,
            VictoryScreen,
        };

        [SerializeField, ReadOnly] private UiType currentUiType;
        [SerializeField, ReadOnly] private UiType lastUiType;
        [SerializeField, ReadOnly] private UiMenu currentUI;

        private void Awake()
        {
            // Init prefabs dictionnary
            uiPrefabs = new Dictionary<UiType, UiMenu>();
            for (int i = 0; i < uiInfos.Length; i++)
            {
                UiInfo uiInfo = uiInfos[i];
                uiPrefabs.Add(uiInfo.type, uiInfo.prefab);
            }

            OpenUiOfType(startUiType);
        }

        public void OpenUiOfType(UiType type)
        {
            if (type == currentUiType)
                return;

            // Destroy current UI if needed
            if (currentUI != null)
            {
                Destroy(currentUI.gameObject);
                currentUI = null;
            }
            
            // Instantiate the new UI
            if (uiPrefabs.TryGetValue(type, out UiMenu menuPrefab))
            {
                currentUI = Instantiate(menuPrefab, transform);
                currentUI.Init(this);
            }
            
            // Update vars
            lastUiType = currentUiType;
            currentUiType = type;
        }

        public void LeaveCurrentUiMenu()
        {
            if (currentUI == null)
                return;

            switch (currentUiType)
            {
                case UiType.Pause:
                    OpenUiOfType(UiType.Game);
                    break;
                case UiType.Options:
                    OpenUiOfType(lastUiType);
                    break;
                default:
                    break;
            }
        }

        public void ExitAllUi()
        {
            if (currentUI == null || currentUiType == UiType.Game)
                return;
            Destroy(currentUI.gameObject);
            currentUI = null;

            lastUiType = currentUiType;
            currentUiType = UiType.Game;
        }

        [System.Serializable]
        public struct UiInfo
        {
            public UiType type;
            public UiMenu prefab;
        }
    }
}