using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OutfoxeedTools
{
    public class UIManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private ScreenType _startScreenType;
        [SerializeField] private ScreenMapping[] _screenMappings;

        [Header("Components")]
        [SerializeField] private EventSystem _eventSystem;
        public EventSystem EventSystem
        {
            get
            {
                if (_eventSystem) _eventSystem = FindFirstObjectByType<EventSystem>();
                return _eventSystem;
            }
        }
        
        [Header("Debug")]
        [SerializeField, ReadOnly] private ScreenBase _currentScreen;
        [SerializeField, ReadOnly] private ScreenType _currentScreenType;
        [SerializeField, ReadOnly] private ScreenType _lastScreenType;

        private void Awake()
        {
            SwitchToScreen(_startScreenType);
        }

        public void SwitchToScreen(ScreenType screenType)
        {
            if (screenType == _currentScreenType)
                return;

            // Destroy current UI if needed
            if (_currentScreen != null)
            {
                Destroy(_currentScreen.gameObject);
                _currentScreen = null;
            }
            
            // Instantiate the new UI
            if (TryGetScreenPrefab(screenType, out ScreenBase menuPrefab))
            {
                _currentScreen = Instantiate(menuPrefab, transform);
                _currentScreen.Init(this);
            }
            
            // Update vars
            _lastScreenType = _currentScreenType;
            _currentScreenType = screenType;
        }

        public void HandleBack()
        {
            SwitchToScreen(GetBackRedirection(_currentScreenType));
            // TODO: unpause game if was in pause screen
        }

        protected virtual ScreenType GetBackRedirection(ScreenType currentScreenType)
        {
            return currentScreenType switch
            {
                ScreenType.Pause => ScreenType.Gameplay,
                _ => _lastScreenType
            };
        }

        private bool TryGetScreenPrefab(ScreenType screenType, out ScreenBase screenPrefab)
        {
            foreach (ScreenMapping screenMapping in _screenMappings)
            {
                if (screenMapping.Type.CompareTo(screenType) == 0)
                {
                    screenPrefab = screenMapping.ScreenPrefab;
                    return true;
                }
            }

            screenPrefab = null;
            return false;
        }
        
        [Serializable]
        public struct ScreenMapping
        {
            [field: SerializeField] public ScreenType Type { get; private set; }
            [field: SerializeField] public ScreenBase ScreenPrefab { get; private set; }
        }
    }
}