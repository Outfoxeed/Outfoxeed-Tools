using System;
using UnityEngine;

namespace OutfoxeedTools
{
    public abstract class ScreenBase : MonoBehaviour
    {
        public abstract bool AllowBack { get; }
        
        [Tooltip("When initializing the UI Menu, this item will be selected in the UI")]
        [SerializeField] private GameObject _firstSelected;
        
        protected UIManager UIManager { get; private set; }
        public void Init(UIManager UIManager)
        {
            this.UIManager = UIManager;
            if(_firstSelected)
                this.UIManager.EventSystem.SetSelectedGameObject(_firstSelected);
        }

        protected virtual void Update()
        {
            if (AllowBack && Input.GetKeyDown(KeyCode.Escape))
            {
                HandleBack();
            }
        }
        
        public void SwitchScreen(ScreenType screenType) => UIManager.SwitchToScreen(screenType);
        public void SwitchScreen(string screenTypeName) => UIManager.SwitchToScreen(Enum.Parse<ScreenType>(screenTypeName)); // method for UnityEvent
        public virtual void HandleBack() => UIManager.HandleBack();
        public void QuitApplication() => Application.Quit();
    }
}