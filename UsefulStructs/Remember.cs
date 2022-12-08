using OutFoxeedTools.Attributes;

namespace OutFoxeedTools.UsefulStructs
{
    [System.Serializable]
    internal struct Remember
    {
        [ReadOnly] private float remember;
        public float duration;

        public Remember(float duration)
        {
            this.duration = duration;
            this.remember = 0;
        }

        public void Trigger() => remember = duration;
        public void DecreaseRemember(float amount) => remember -= amount;
        public void Reset() => remember = 0;
            
        public bool IsRemembering() => remember > 0;
    }
}