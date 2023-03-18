using System;
using UnityEngine;

namespace OutfoxeedTools.MonoBehaviourBase
{
    public abstract class GameManagerBase<T> : SingletonBase<T> where T : MonoBehaviour
    {
        // Pause system
        #region Pause
        public bool Paused { get; protected set; }
        protected float wantedTimeScale = 1f;
        public void TogglePause() => TrySetPause(!Paused);
        public void TrySetPause(bool pause)
        {
            if (Paused == pause)
                return;
            SetPause(pause);
        }
        private void SetPause(bool pause)
        {
            Paused = pause;

            // Time scale gestion
            if (pause) wantedTimeScale = Time.timeScale;
            Time.timeScale = Paused ? 0f : wantedTimeScale;

            OnSetPaused();
            OnPauseUpdated?.Invoke(Paused);
        }
        protected virtual void OnSetPaused()
        {
        }
        public event Action<bool> OnPauseUpdated;
        #endregion
        // // /

        // Game State
        #region GameStates
        public enum GameState
        {
            Starting,
            Game,
            Finishing,
            Finished,
        };

        public GameState LastGameState { get; protected set; }
        private GameState currentGameState = GameState.Starting;
        public GameState CurrentGameState
        {
            get => currentGameState;
            set
            {
                if (currentGameState == value)
                    return;
                LastGameState = currentGameState;
                currentGameState = value;
                
                OnGameStateSet();
                OnGameStateChanged?.Invoke(currentGameState);
            }
        }
        protected virtual void OnGameStateSet()
        {
            
        }
        public event Action<GameState> OnGameStateChanged;
        #endregion
        // // //
        
        // Cleaner reference to Camera.main
        public Camera MainCam { get; protected set; }


        protected override void Awake()
        {
            base.Awake();

            MainCam = Camera.main;
            wantedTimeScale = 1f;
            SetPause(false);
        }
    }
}