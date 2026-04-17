namespace OutfoxeedTools
{
    public class OptionsScreen : ScreenBase
    {
        public override bool AllowBack => true;
        
        public virtual void OnMainVolumeSliderChanged(float value) {}
        public virtual void OnSFXVolumeSliderChanged(float value) {}
        public virtual void OnMusicVolumeSliderChanged(float value) {}
        
        public virtual void OnSensitivitySliderChanged(float value) {}
    }
}