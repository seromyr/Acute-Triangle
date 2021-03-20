using Constants;
using System;
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

    // Player ingame HUD
    private Image playerHealth, hpFrameOverlay;
    private Color hpOverlayColor, highlightColor;
    private float colorPulsingTimer;

    //boss ingameHud
    private Image currentBHealth, nextBHealth;
    private Image julietteBossHealth;
    private Image ragazzinoBossHealth;
    private Image warwickBossHealth;
    private Image navelBossHealth;
    private Image minotaurBossHealth;
    private Image pupuMoxieBossHealth;
    private Image gearboxBossHealth;
    private Image beholderBossHealth;

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
        Player.main.OnDamage += UpdateGameplayUI;
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

        playerHealth = transform.Find("PlayerHP").GetComponentInChildren<Image>();
        hpFrameOverlay = transform.Find("HpOverlay").GetComponentInChildren<Image>();
        hpOverlayColor = hpFrameOverlay.color;
        hpOverlayColor.a = 0;
        hpFrameOverlay.color = hpOverlayColor;
        highlightColor = new Color(0.8f, 0, 0, 0);
        colorPulsingTimer = 0;

        julietteBossHealth = transform.Find("JulietteHP").GetComponentInChildren<Image>();
        ragazzinoBossHealth = transform.Find("RagazzinoHP").GetComponentInChildren<Image>();
        warwickBossHealth = transform.Find("WarwickHP").GetComponentInChildren<Image>();
        navelBossHealth = transform.Find("NavelHP").GetComponentInChildren<Image>();
        minotaurBossHealth = transform.Find("MinotaurHP").GetComponentInChildren<Image>();
        pupuMoxieBossHealth = transform.Find("Pupu&MoxieHP").GetComponentInChildren<Image>();
        gearboxBossHealth = transform.Find("GearboxHP").GetComponentInChildren<Image>();
        beholderBossHealth = transform.Find("BeholderHP").GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (colorPulsingTimer > 0)
        {
            colorPulsingTimer -= Time.deltaTime;
            highlightColor.a = hpOverlayColor.a;
            hpFrameOverlay.color = Color.Lerp(hpFrameOverlay.color, highlightColor, Time.deltaTime * 5);

            if (colorPulsingTimer <= 0)
            {
                hpFrameOverlay.color = hpOverlayColor;
            }
        }
    }

    private void UpdateGameplayUI(object sender, EventArgs e)
    {
        playerHealth.fillAmount = Player.main.Health / Player.main.MaxHealth;
        hpOverlayColor.a = 1 - playerHealth.fillAmount;
        //Activate Hp Overlay Pulsing
        colorPulsingTimer = 0.2f;
    }

    //get boss hp bar and get image
    //get bosses health (current and max)
    //update it

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

    public void UsePause() => PauseGame();

    public void ResetPlayerHUD()
    {
        playerHealth.fillAmount = Player.main.Health / Player.main.MaxHealth; ;
        hpOverlayColor.a = 1 - playerHealth.fillAmount;
        hpFrameOverlay.color = hpOverlayColor;
        colorPulsingTimer = 0;
    }
}
