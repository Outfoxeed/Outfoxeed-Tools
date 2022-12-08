using OutFoxeedTools.Attributes;
using UnityEngine;

namespace OutFoxeedTools.UsefulStructs
{
    [System.Serializable]
    public class AutoCooldown : Cooldown
    {
        public AutoCooldown(float duration, bool setCooldownReady = false) : base(duration, setCooldownReady)
        {
        }

        public override bool IsReady() => value <= Time.time;
        public override void SetReady() => value = 0;
        public override void Reset() => value = Time.time + Duration;
    }
}