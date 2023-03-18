using System;
using UnityEngine;

namespace OutfoxeedTools.UI
{
    public class UiMenu : MonoBehaviour
    {
        [Tooltip("When initializing the UI Menu, this item will be selected in the UI")]
        [SerializeField] private GameObject FirstSelected;
        
        protected UiManager uiManager;
        public void Init(UiManager uiManager)
        {
            this.uiManager = uiManager;
            if(FirstSelected)
                uiManager.EventSystem.SetSelectedGameObject(FirstSelected);
        }

        protected void OpenUiOfType(UiManager.UiType type) => uiManager.OpenUiOfType(type);
        
        #region Methods for Ui buttons
        public void OpenUiOfType(string typeName)
        {
            if (Enum.TryParse(typeName, out UiManager.UiType type))
            {
                uiManager.OpenUiOfType(type);
            }
        }
        public void LeaveMenu() => uiManager.LeaveCurrentUiMenu();
        public void ExitAllUi() => uiManager.ExitAllUi();
        public void QuitApplication() => Application.Quit();
        #endregion
    }
}