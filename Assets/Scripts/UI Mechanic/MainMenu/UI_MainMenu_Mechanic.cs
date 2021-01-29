﻿using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu_Mechanic : MonoBehaviour
{
    public static UI_MainMenu_Mechanic main;

    // Canvas
    private Canvas canvas;

    // Splash Screen group
    private Image splashBkg;
    private Text splashText;
    // -------------------

    // Main Menu group
    private Image mainMenuBkg;
    private Text mainMenuTitle;
    private Button newGame, quitGame;
    // -------------------

    private void Awake()
    {
        // Make the Main Menu a Singleton
        Singletonize();

        // Canvas
        canvas = GetComponent<Canvas>();

        // Set up screen
        SplashScreenSetup();
        MainMenuSetup();
    }

    private void Start()
    {
        // Disable Main Menu Background
        mainMenuBkg.gameObject.SetActive(false);
    }

    private void Singletonize()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("Main Menu created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void SplashScreenSetup()
    {
        splashBkg = transform.Find("SplashScreen").GetComponent<Image>();
        splashText = splashBkg.transform.Find("Text").GetComponent<Text>();

        // Splash BG is black
        splashBkg.color = Color.black;

        // Display text
        splashText.text = "Press Enter to start";
    }

    private void SplashTextPulsing()
    {
        // Text Pulsing effect
        splashText.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
    }

    private void MainMenuSetup()
    {
        mainMenuBkg = transform.Find("MainMenuScreen").GetComponent<Image>();
        mainMenuTitle = mainMenuBkg.transform.Find("Title").GetComponent<Text>();

        // Main Menu BG is black
        mainMenuBkg.color = Color.black;

        // Display title
        mainMenuTitle.text = Version.NAME + "\n" + Version.CURRENTVERSION;
        mainMenuTitle.color = Color.white;

        // Add function to New Game Button
        newGame = mainMenuBkg.transform.Find("NewGame").GetComponent<Button>();
        newGame.onClick.AddListener(StartNewGame);

        // Add function to Quit Game Button
        quitGame = mainMenuBkg.transform.Find("Quit").GetComponent<Button>();
        quitGame.onClick.AddListener(QuitGame);
        // Disable if webGL
        #if UNITY_WEBGL
        quitGame.gameObject.SetActive(false);
        #endif
    }

    private void StartNewGame()
    {
        GameManager.main.StartNewGame();
    }

    private void QuitGame()
    {
        GameManager.main.QuitGame();
    }

    private void SplashScreenInputReading(KeyCode key)
    {
        if (Input.GetKey(key) || Input.GetMouseButton(0))
        {
            splashBkg.gameObject.SetActive(false);
            mainMenuBkg.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            splashBkg.gameObject.SetActive(true);
            mainMenuBkg.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        SplashTextPulsing();
        SplashScreenInputReading(KeyCode.Return);
    }

    public void SetActiveCanvas(bool value)
    {
        canvas.enabled = value;
    }
}
