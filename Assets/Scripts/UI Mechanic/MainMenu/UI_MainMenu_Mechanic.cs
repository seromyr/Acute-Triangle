using Constants;
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
    private Button newGame, selectLevel, credits, quitGame;
    // -------------------

    // Level Selection group
    private Image levelSelectBkg;
    private Button level_1, level_2, level_3, level_4, level_5, level_6, level_7, level_8, level_9, level_10, level_11, level_12, level_13, level_14, level_15, level_16, level_17, level_18, level_19, level_20, levelBackToMain;
    // -------------------

    // Credits Screen group
    private Image creditsBkg;
    private Button creditsBackToMain;
    // -------------------

    private AudioSource menuSelect;
    private void Awake()
    {
        // Make the Main Menu a Singleton
        Singletonize();

        // Canvas
        canvas = GetComponent<Canvas>();

        // AudioSource 
        menuSelect = GetComponent<AudioSource>();

        // Set up screens
        SplashScreenSetup();
        MainMenuSetup();
        LevelSelectScreenSetup();
        CreditsScreenSetup();
    }

    private void Start()
    {
        // Disable Main Menu Background
        mainMenuBkg.gameObject.SetActive(false);

        // Disable Level Select Background
        levelSelectBkg.gameObject.SetActive(false);

        // Disable Credits Screen Background
        creditsBkg.gameObject.SetActive(false);
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

    #region Splash Screen
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
    #endregion

    #region Main Menu
    private void MainMenuSetup()
    {
        mainMenuBkg = transform.Find("MainMenuScreen").GetComponent<Image>();
        mainMenuTitle = mainMenuBkg.transform.Find("Title").GetComponent<Text>();

        // Main Menu BG is black
        //mainMenuBkg.color = Color.black;

        // Display title
        mainMenuTitle.text = "v" + Version.CURRENTVERSION + "build" + Version.BUILD + "." + Version.DATE;
        mainMenuTitle.color = Color.gray;

        // Add function to New Game Button
        newGame = mainMenuBkg.transform.Find("NewGame").GetComponent<Button>();
        newGame.onClick.AddListener(StartNewGame);

        // Add function to Credits Button
        selectLevel = mainMenuBkg.transform.Find("SelectLevel").GetComponent<Button>();
        selectLevel.onClick.AddListener(SelectLevel);
        
        // Add function to Credits Button
        credits = mainMenuBkg.transform.Find("Credits").GetComponent<Button>();
        credits.onClick.AddListener(ViewCredits);

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
        PlaySound();
    }

    private void SelectLevel()
    {
        levelSelectBkg.gameObject.SetActive(true);
        PlaySound();
    }

    private void ViewCredits()
    {
        creditsBkg.gameObject.SetActive(true);
        PlaySound();
    }

    private void QuitGame()
    {
        PlaySound();
        GameManager.main.QuitGame();
    }

    #endregion

    #region Level Select Screen
    private void LevelSelectScreenSetup()
    {
        levelSelectBkg = transform.Find("LevelSelectionScreen").GetComponent<Image>();

        // Add function to Level Select Buttons
        level_1 = levelSelectBkg.transform.Find("Level 1").GetComponent<Button>();
        level_2 = levelSelectBkg.transform.Find("Level 2").GetComponent<Button>();
        level_3 = levelSelectBkg.transform.Find("Level 3").GetComponent<Button>();
        level_4 = levelSelectBkg.transform.Find("Level 4").GetComponent<Button>();
        level_5 = levelSelectBkg.transform.Find("Level 5").GetComponent<Button>();
        level_6 = levelSelectBkg.transform.Find("Level 6").GetComponent<Button>();
        level_7 = levelSelectBkg.transform.Find("Level 7").GetComponent<Button>();
        level_8 = levelSelectBkg.transform.Find("Level 8").GetComponent<Button>();
        level_9 = levelSelectBkg.transform.Find("Level 9").GetComponent<Button>();
        level_10 = levelSelectBkg.transform.Find("Level 10").GetComponent<Button>();
        level_11 = levelSelectBkg.transform.Find("Level 11").GetComponent<Button>();
        level_12 = levelSelectBkg.transform.Find("Level 12").GetComponent<Button>();
        level_13 = levelSelectBkg.transform.Find("Level 13").GetComponent<Button>();
        level_14 = levelSelectBkg.transform.Find("Level 14").GetComponent<Button>();
        level_15 = levelSelectBkg.transform.Find("Level 15").GetComponent<Button>();
        level_16 = levelSelectBkg.transform.Find("Level 16").GetComponent<Button>();
        level_17 = levelSelectBkg.transform.Find("Level 17").GetComponent<Button>();
        level_18 = levelSelectBkg.transform.Find("Level 18").GetComponent<Button>();
        level_19 = levelSelectBkg.transform.Find("Level 19").GetComponent<Button>();
        level_20 = levelSelectBkg.transform.Find("Level 20").GetComponent<Button>();

        level_1.onClick.AddListener(() => GoToLevel(1));
        level_2.onClick.AddListener(() => GoToLevel(2));
        level_3.onClick.AddListener(() => GoToLevel(3));
        level_4.onClick.AddListener(() => GoToLevel(4));
        level_5.onClick.AddListener(() => GoToLevel(5));
        level_6.onClick.AddListener(() => GoToLevel(6));
        level_7.onClick.AddListener(() => GoToLevel(7));
        level_8.onClick.AddListener(() => GoToLevel(8));
        level_9.onClick.AddListener(() => GoToLevel(9));
        level_10.onClick.AddListener(() => GoToLevel(10));
        level_11.onClick.AddListener(() => GoToLevel(11));
        level_12.onClick.AddListener(() => GoToLevel(12));
        level_13.onClick.AddListener(() => GoToLevel(13));
        level_14.onClick.AddListener(() => GoToLevel(14));
        level_15.onClick.AddListener(() => GoToLevel(15));
        level_16.onClick.AddListener(() => GoToLevel(16));
        level_17.onClick.AddListener(() => GoToLevel(17));
        level_18.onClick.AddListener(() => GoToLevel(18));
        level_19.onClick.AddListener(() => GoToLevel(19));
        level_20.onClick.AddListener(() => GoToLevel(20));

        // Lock levels
        level_9.interactable = true;
        level_10.interactable = true;
        level_11.interactable = true;
        level_12.interactable = true;
        level_13.interactable = true;
        level_14.interactable = true;
        level_15.interactable = true;


        // Add function to Back To Main Menu Button
        levelBackToMain = levelSelectBkg.transform.Find("BackToMainMenu").GetComponent<Button>();
        levelBackToMain.onClick.AddListener(LevelSelectBackToMainMenu);
    }

    private void GoToLevel(int levelNumber)
    {
        //Debug.LogError(levelNumber);
        GameManager.main.SetNextLevel(levelNumber);
        PlaySound();
    }


    private void LevelSelectBackToMainMenu()
    {
        levelSelectBkg.gameObject.SetActive(false);
        PlaySound();
    }
    #endregion

    #region Credits Screen
    private void CreditsScreenSetup()
    {
        creditsBkg = transform.Find("CreditsScreen").GetComponent<Image>();

        // Add function to Back To Main Menu Button
        creditsBackToMain = creditsBkg.transform.Find("BackToMainMenu").GetComponent<Button>();
        creditsBackToMain.onClick.AddListener(CreditsBackToMainMenu);
    }

    private void CreditsBackToMainMenu()
    {
        creditsBkg.gameObject.SetActive(false);
        PlaySound();
    }
    #endregion

    private void Update()
    {
        SplashTextPulsing();
        SplashScreenInputReading(KeyCode.Return);
    }
    private void PlaySound()
    {
        menuSelect.PlayOneShot(menuSelect.clip);
    }

    public void SetActiveCanvas(bool value)
    {
        canvas.enabled = value;
    }

    public void UnlockLevel(int levelID)
    {
        switch (levelID)
        {
            default:
            case 9:
                level_9.interactable = true;
                break;
            case 10:
                level_10.interactable = true;
                break;
            case 11:
                level_11.interactable = true;
                break;
            case 12:
                level_12.interactable = true;
                break;
            case 13:
                level_13.interactable = true;
                break;
            case 14:
                level_14.interactable = true;
                break;
            case 15:
                level_15.interactable = true;
                break;
        }
    }
}
