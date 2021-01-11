using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Constants;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public static PlayerEntity player;

    [SerializeField]
    private GameState gameState, desinationState;

    private bool stateUpdating;

    private int nextLevel;

    [SerializeField]
    private int playerHealth;

    [SerializeField, Header("Money on game start")]
    private int money;
    public int CheckWallet { get { return money; } }

    private void Awake()
    {
        // Make the Game Manager a Singleton
        SingletonizeGameManager();

        // Make player a Singleton
        SingletonizePlayer();

        // Setup before loading the Main Menu
        InitialGameSetup();
    }

    void Start()
    {
        stateUpdating = true;

        // Hide loading screen
        UI_LoadingScreen_Mechanic.main.RequestFadeOut(2);
        
        UI_Control.main.gameObject.SetActive(false);

        nextLevel = 0;
    }

    private void SingletonizeGameManager()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("Game Manager created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void SingletonizePlayer()
    {
        if (player == null)
        {
            // Construct player
            player = new PlayerEntity();
            player.Avatar.gameObject.SetActive(false);
            player.Reset();
            player.Avatar.transform.GetComponent<PlayerController>().LoadJoystick();

            DontDestroyOnLoad(player.Avatar);
            Debug.Log("Player was created");
        }
        else if (player != new PlayerEntity())
        {
            Destroy(player.Avatar);
        }
    }

    private void InitialGameSetup()
    {
        // Construct player
        //new PlayerEntity();
        //PlayerMonitor.main.gameObject.SetActive(false);
        //PlayerMonitor.main.Reset();
        //PlayerMonitor.main.transform.GetComponent<PlayerController>().LoadJoystick();

        // Load camera in scene
        //GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().LookAt = (PlayerMonitor.main.transform);
        //GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = (PlayerMonitor.main.transform);

        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().LookAt = (player.Avatar.transform);
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = (player.Avatar.transform);

        // Set initial game state
        gameState = GameState.LOADING;
        desinationState = GameState.START;
    }

    private void OnLevelWasLoaded()
    {
        UI_InGameMenu_Mechanic.main.SendInstruction(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        GameStateMonitoring();
    }

    private void GameStateMonitoring()
    {
        if (stateUpdating)
        {
            switch (gameState)
            {
                case GameState.LOADING: Perform___LOADING___Routines(); break;
                case GameState.START:   Perform____START____Routines(); break;
                case GameState.NEW:     Perform_____NEW_____Routines(); break;
                case GameState.NEXT:    Perform_____NEXT____Routines(); break;
                case GameState.RUNNING: Perform___RUNNING___Routines(); break;
                case GameState.PAUSE:   Perform____PAUSE____Routines(); break;
                case GameState.WIN:     Perform_____WIN_____Routines(); break;
                case GameState.LOSE:    Perform_____LOSE____Routines(); break;
            }
        }
        //--------------------
        stateUpdating = false;
    }

    private void Perform___LOADING___Routines()
    {
        Time.timeScale = 1;

        // Disable player
        player.Avatar.SetActive(false);

        // Disable control panel
        UI_Control.main.gameObject.SetActive(false);

        // Disable In Game Menu
        UI_InGameMenu_Mechanic.main.SetActiveCanvas(false);

        // Disable Win Panel
        UI_WinPanel.main.gameObject.SetActive(false);

        // Next step, switch to destination state, on default is START
        StartCoroutine(SwitchState(desinationState, 1));
    }

    private void Perform____START____Routines()
    {
        SceneManager.LoadScene(SceneName.MAINMENU);
        //Time.timeScale = 1;

        // Activate Main Menu UI
        UI_MainMenu_Mechanic.main.SetActiveCanvas(true);
    }

    private void Perform_____NEW_____Routines()
    {
        // Next level reset
        nextLevel = 0;

        Perform_____NEXT____Routines();
    }

    private void Perform_____NEXT____Routines()
    {
        // Load next level
        SceneManager.LoadScene(Map.NAME + nextLevel);
        Debug.Log("Next level: Level_0" + nextLevel);

        // Activate player
        //PlayerMonitor.main.gameObject.SetActive(true);
        //PlayerMonitor.main.Reset();

        player.Avatar.SetActive(true);
        player.Reset();

        // Deactivate Main Menu
        UI_MainMenu_Mechanic.main.SetActiveCanvas(false);

        // Activate In Game Menu
        UI_InGameMenu_Mechanic.main.SetActiveCanvas(true);

        // Next step
        desinationState = GameState.RUNNING;
        StartCoroutine(SwitchState(desinationState, 0));
        StartCoroutine(UI_LoadingScreen_Mechanic.main.RequestFadeOut(5, 0.5f));
    }

    private void Perform___RUNNING___Routines()
    {
        Time.timeScale = 1;
        UI_Control.main.gameObject.SetActive(true);
    }

    private void Perform____PAUSE____Routines()
    {
        UI_Control.main.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    private void Perform_____WIN_____Routines()
    {
        // Show win panel
        UI_WinPanel.main.gameObject.SetActive(true);

        nextLevel++;

        //Cheat
        if (nextLevel == 3)
        {
            desinationState = GameState.START;
            StartCoroutine(SwitchState(GameState.LOADING, 2.5f));
            UI_InGameMenu_Mechanic.main.SendInstruction("GAME OVER");
            StartCoroutine(UI_LoadingScreen_Mechanic.main.RequestFadeOut(5, 4.5f));
        }
        else
        {
            // Increase next level ID
            desinationState = GameState.NEXT;
            StartCoroutine(SwitchState(GameState.LOADING, 3));
        }

        //desinationState = GameState.NEXT;
    }

    private void Perform_____LOSE____Routines()
    {
    }

    private IEnumerator SwitchState(GameState state, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameState = state;
        stateUpdating = true;
    }

    public void GoToMainMenu()
    {
        stateUpdating = true;

        // Show loading screen
        UI_LoadingScreen_Mechanic.main.RequestFadeIn(2);

        desinationState = GameState.START;
        StartCoroutine(SwitchState(GameState.LOADING, 0));
        StartCoroutine(UI_LoadingScreen_Mechanic.main.RequestFadeOut(5, 1.5f));
    }

    public void StartNewGame()
    {
        stateUpdating = true;
        // Show loading screen
        UI_LoadingScreen_Mechanic.main.RequestFadeIn(2);

        desinationState = GameState.NEW;
        StartCoroutine(SwitchState(GameState.LOADING, 1));
    }

    public void ResumeGame()
    {
        stateUpdating = true;
        gameState = GameState.RUNNING;
    }

    public void PauseGame()
    {
        stateUpdating = true;
        gameState = GameState.PAUSE;
    }

    public void WinGame()
    {
        stateUpdating = true;

        // Disable In Game Menu
        UI_InGameMenu_Mechanic.main.SetActiveCanvas(false);

        gameState = GameState.WIN;
        UI_InGameMenu_Mechanic.main.SendInstruction("Enemy defeated");

        // Show loading screen
        StartCoroutine(UI_LoadingScreen_Mechanic.main.RequestFadeIn(2, 2));
    }

    public void LoseGame()
    {
        
    }
}