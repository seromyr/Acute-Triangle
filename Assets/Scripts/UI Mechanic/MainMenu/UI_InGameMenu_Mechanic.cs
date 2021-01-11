using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameMenu_Mechanic : MonoBehaviour
{
    public static UI_InGameMenu_Mechanic main;

    private Canvas canvas;

    private GameObject inGameMenuScreen;
    private Button pauseGame, resumeGame, returnToMainMenu;

    private Text instructionMessage;

    private void Awake()
    {
        // Make the In Game Menu a Singleton
        SingletonMaker();

        // Canvas
        canvas = GetComponent<Canvas>();

        // Set up in game menu screen
        InGameMenuSetup();
    }

    private void Start()
    {
        inGameMenuScreen.SetActive(false);
    }
    private void SingletonMaker()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("In Game Menu created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void InGameMenuSetup()
    {
        pauseGame = transform.Find("Pause").GetComponent<Button>();
        pauseGame.onClick.AddListener(PauseGame);

        inGameMenuScreen = transform.Find("IngameMenuScreen").gameObject;

        resumeGame = inGameMenuScreen.transform.Find("UnPause").GetComponent<Button>();
        resumeGame.onClick.AddListener(ResumeGame);

        returnToMainMenu = inGameMenuScreen.transform.Find("GoToMainMenu").GetComponent<Button>();
        returnToMainMenu.onClick.AddListener(GoToMainMenu);

        instructionMessage = transform.Find("Instruction").GetComponentInChildren<Text>();
    }

    private void PauseGame()
    {
        GameManager.main.PauseGame();
        inGameMenuScreen.SetActive(true);
    }

    private void ResumeGame()
    {
        GameManager.main.ResumeGame();
        inGameMenuScreen.SetActive(false);
    }

    private void GoToMainMenu()
    {
        inGameMenuScreen.SetActive(false);
        GameManager.main.GoToMainMenu();
    }
    public void SetActiveCanvas(bool value)
    {
        canvas.enabled = value;
    }

    public void SendInstruction(string text)
    {
        instructionMessage.text = text;
    }
}
