using UnityEngine.SceneManagement;

namespace OutfoxeedTools
{
    public class MainMenuScreen : ScreenBase
    {
        public override bool AllowBack => false;
        
        public void LoadNextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}