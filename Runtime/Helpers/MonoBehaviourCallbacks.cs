using System;
using UnityEngine;
using UnityEngine.Events;

namespace OutfoxeedTools
{
    public class MonoBehaviourCallbacks : MonoBehaviour
    {
        private enum Phase
        {
            Awake,
            OnEnable,
            OnDisable,
            Start,
            OnDestroy,
            Update,
            FixedUpdate,
            LateUpdate
        }
    
        [Serializable]
        private struct CallbackPair
        {
            [field: SerializeField] public Phase Phase { get; private set; }
            [field: SerializeField] public UnityEvent Event { get; private set; }
        }

        [SerializeField] private CallbackPair[] _callbacks;

        private void Awake() => TriggerCallback(Phase.Awake);
        private void OnEnable() => TriggerCallback(Phase.OnEnable);
        private void OnDisable() => TriggerCallback(Phase.OnDisable);
        private void Start() => TriggerCallback(Phase.Start);
        private void OnDestroy() => TriggerCallback(Phase.OnDestroy);
        private void Update() => TriggerCallback(Phase.Update);
        private void FixedUpdate() => TriggerCallback(Phase.FixedUpdate);
        private void LateUpdate() => TriggerCallback(Phase.LateUpdate);
        
        private void TriggerCallback(Phase phase)
        {
            if (_callbacks is null)
            {
                Debug.LogError($"Callbacks of '{GetType()}' are not initialized", this);
                return;
            }
        
            foreach (CallbackPair callbackPair in _callbacks)
            {
                if (callbackPair.Phase == phase)
                {
                    callbackPair.Event.Invoke();
                    return;
                }
            }
        }
    }
}