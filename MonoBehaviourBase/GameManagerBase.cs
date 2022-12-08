using UnityEngine;

namespace OutFoxeedTools.MonoBehaviourBase
{
    public abstract class GameManagerBase<T> : SingletonBase<T> where T : MonoBehaviour
    {
        // Cleaner reference to Camera.main
        public Camera MainCam { get; protected set; }

        // Pause system
        private bool paused;
        public bool IsPaused => paused;
        private float wantedTimeScale;
        public void TogglePause() => SetPause(!paused);
        public void SetPause(bool pause)
        {
            if (paused == pause)
                return;
            paused = pause;

            // Time scale gestion
            if (pause) wantedTimeScale = Time.timeScale;
            Time.timeScale = paused ? 0f : wantedTimeScale;

            OnSetPaused();
        }
        protected virtual void OnSetPaused()
        {
        }

        // Game State
        public enum GameState
        {
            Game,
            Pause,
            End
        };

        private GameState lastGameState;
        private GameState currentGameState = GameState.Game;

        public GameState CurrentGameState
        {
            get => currentGameState;
            set
            {
                if (currentGameState == value)
                    return;
                lastGameState = currentGameState;
                currentGameState = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            MainCam = Camera.main;
            wantedTimeScale = 1f;
        }
    }
}