using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class GameManager : Singleton<GameManager>
    {
        //[SerializeField] DataManager dataManager;
        [SerializeField] UIManager uiManager;
        [SerializeField] GameController gameController;

        public static GameState currentState { get; private set; }
        public static GameMode currentMode { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            //dataManager.Initialize();
        }
        private void Start()
        {
            uiManager.Initialize(this);
            //if (Scene.isFirstLoad)
            //{
            //    AudioManager.Instance.PlayMusic("background music 2", 1, true);
            //    uiManager.ActiveScreen<MainMenuUI>(true, true, true);
            //    gameController.SetActive(false);
            //    SwitchGameState(GameState.NONE);
            //}
            //else
            {
                AudioManager.Instance.PlayMusic("background music 1", 1, true);
                //gameController.Initialize();
                //gameController.SetActive(true);
                //uiManager.ActiveScreen<InGameUI>();
                SwitchGameState(GameState.PLAY);
            }
        }
        private void Update()
        {
            if (currentState == GameState.PLAY)
            {
                gameController.UpdateLogic();
            }
        }
        private void LateUpdate()
        {
            if (currentState == GameState.PLAY)
            {
                gameController.UpdateLate();
            }
    }
    public void SwitchGameState(GameState newState)
        {
            Debug.Log($"<color=#9CDCE1>=> STATUS _ GAME-STATE: {newState}</color>");
            currentState = newState;
        }
        public void SwitchGameMode(GameMode newMode)
        {
            Debug.Log($"<color=#9CDCE1>=> STATUS _ GAME-MODE: {newMode}</color>");
            currentMode = newMode;
        }

        public void Delay(float duration, Action onComplete)
        {
            StartCoroutine(IECallBack());
            IEnumerator IECallBack()
            {
                float t = duration;
                while (t > 0)
                {
                    t -= Time.deltaTime;
                    yield return null;
                }
                onComplete?.Invoke();
            }
        }
    }
    public enum GameState
    {
        NONE, TUTORIAL, PLAY, LOSE, WIN, PAUSE
    }
    public enum GameMode
    {
        IN_GAME
    }

