namespace OutFoxeedTools.UsefulStructs
{
    [System.Serializable]
    public class ManualCooldown : Cooldown
    {
        public ManualCooldown(float duration, bool setCooldownReady = false) : base(duration, setCooldownReady)
        {
        }
        
        public override bool IsReady() => value <= 0f;
        public override void SetReady() => value = 0f;
        public override void Reset() => value = Duration;
        public void Decrease(float deltaTime) => value -= deltaTime;

    }
}