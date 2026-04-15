using UnityEngine.SceneManagement;

namespace OutfoxeedTools
{
    public class MainMenuBase : UiMenu
    {
        public virtual void PlayButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}