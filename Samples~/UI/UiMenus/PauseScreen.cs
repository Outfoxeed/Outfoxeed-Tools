namespace OutfoxeedTools
{
    public class PauseScreen : ScreenBase
    {
        public override bool AllowBack => true;
        
        public virtual void OnResumeGameButtonClicked() => HandleBack();
    }
}