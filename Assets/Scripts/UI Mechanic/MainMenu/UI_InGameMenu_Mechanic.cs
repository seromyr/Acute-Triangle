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

    //Boss HP bars
    private Image julietteHP;
    private Image ragazzinoHP;
    private Image warwickHP;
    private Image navelHP;
    private Image minotaurHP;
    private Image pupuMoxieHP;
    private Image beholderHP;
    private Image gearboxHP;
    private Image currentBossHP;

    private float bossMaxHealth;
    private float bossCurrentHealth;

    //AudioSource
    private AudioSource menuSelect;

    // Cheat box
    private Toggle godMode;

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
        menuSelect = GetComponent<AudioSource>();
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

        julietteHP = transform.Find("JulietteHP").GetComponentInChildren<Image>();
        ragazzinoHP = transform.Find("RagazzinoHP").GetComponentInChildren<Image>();
        warwickHP = transform.Find("WarwickHP").GetComponentInChildren<Image>();
        navelHP = transform.Find("NavelHP").GetComponentInChildren<Image>();
        minotaurHP = transform.Find("MinotaurHP").GetComponentInChildren<Image>();
        pupuMoxieHP = transform.Find("Pupu&MoxieHP").GetComponentInChildren<Image>();
        beholderHP = transform.Find("BeholderHP").GetComponentInChildren<Image>();
        gearboxHP = transform.Find("GearboxHP").GetComponentInChildren<Image>();

        // Remember to remove cheat
        godMode = inGameMenuScreen.transform.Find("GodMode").GetComponentInChildren<Toggle>();
        godMode.isOn = false;
        godMode.onValueChanged.AddListener(delegate { SwitchGodMode(godMode); });
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

    private void DisableBossHPBars()
    {
        julietteHP.gameObject.SetActive(false);
        ragazzinoHP.gameObject.SetActive(false);
        warwickHP.gameObject.SetActive(false);
        navelHP.gameObject.SetActive(false);
        minotaurHP.gameObject.SetActive(false);
        pupuMoxieHP.gameObject.SetActive(false);
        beholderHP.gameObject.SetActive(false);
        gearboxHP.gameObject.SetActive(false);
    }

    public void ChangeBossHPBars(int bossNum)
    {
        DisableBossHPBars();
        switch (bossNum)
        {
            case 1:
                currentBossHP = julietteHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 2:
                currentBossHP = ragazzinoHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 3:
                currentBossHP = warwickHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 4:
                currentBossHP = navelHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 5:
                currentBossHP = minotaurHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 6:
                currentBossHP = pupuMoxieHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 7:
                currentBossHP = beholderHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            case 8:
                currentBossHP = gearboxHP;
                currentBossHP.gameObject.SetActive(true);
                break;
            default:
                currentBossHP.gameObject.SetActive(false);
                break;
        }
    }

    public void SetBossHPMax(float max)
    {
        bossMaxHealth = max;
    }
    public void SetBossHPCurrent(float current)
    {
        bossCurrentHealth = current;
    }

    public void UpdateBossHPCounter(float damage)
    {
        bossCurrentHealth -= damage;
        if (bossCurrentHealth < 0f) bossCurrentHealth = 0f;

        currentBossHP.fillAmount = bossCurrentHealth / bossMaxHealth;
    }

    private void UpdateGameplayUI(object sender, EventArgs e)
    {
        playerHealth.fillAmount = Player.main.Health / Player.main.MaxHealth;
        hpOverlayColor.a = 1 - playerHealth.fillAmount;
        //Activate Hp Overlay Pulsing
        colorPulsingTimer = 0.2f;
    }

    private void PlaySound()
    {
        menuSelect.PlayOneShot(menuSelect.clip);
    }

    private void PauseGame()
    {
        GameManager.main.PauseGame();
        inGameMenuScreen.SetActive(true);
        PlaySound();
    }

    private void ResumeGame()
    {
        GameManager.main.ResumeGame();
        inGameMenuScreen.SetActive(false);
        PlaySound();
    }

    private void GoToMainMenu()
    {
        inGameMenuScreen.SetActive(false);
        GameManager.main.GoToMainMenu();
        PlaySound();
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

    private void SwitchGodMode(Toggle change)
    {
        Player.main.MakePlayerInvincile(godMode.isOn);
    }
}
