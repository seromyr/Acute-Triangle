using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LosePanel : MonoBehaviour
{
    public static UI_LosePanel main;

    private Button retry, mainMenu;

    private void Awake()
    {
        // Make the Control Panel a Singleton
        Singletonize();

        // Setup button
        ButtonSetup();
    }

    private void Singletonize()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("Lose Panel created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void ButtonSetup()
    {
        transform.Find("Retry").TryGetComponent(out retry);
        retry.onClick.AddListener(Retry);

        transform.Find("Quit").TryGetComponent(out mainMenu);
        mainMenu.onClick.AddListener(ReturnToMainMenu);
    }

    private void Retry()
    {
        GameManager.main.RetryLevel();
    }

    private void ReturnToMainMenu()
    {
        GameManager.main.GoToMainMenu();
    }
}
