using UnityEngine;
using UnityEngine.SceneManagement;

namespace OutFoxeedTools.UI.UiMenus
{
    public class MainMenuBase : UiMenu
    {
        public virtual void PlayButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}