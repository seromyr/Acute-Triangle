using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScreen_Mechanic : MonoBehaviour
{
    public static UI_LoadingScreen_Mechanic main;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Text text;

    private bool fadeIn, fadeOut;
    private float fadeSpeed;

    private void Awake()
    {
        // Make the Main Menu a Singleton
        SingletonMaker();

        // Canvas
        canvas = GetComponent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas.sortingOrder = 3;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = false;
        fadeIn = false;
        fadeOut = false;

        text = transform.Find("Panel").transform.GetComponentInChildren<Text>();
    }

    private void SingletonMaker()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            main = this;
            Debug.Log("Loading Screen created");
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (fadeIn) FadeIn();
        if (fadeOut) FadeOut();
    }
    public void RequestFadeIn(float speed)
    {
        fadeIn = true;
        fadeOut = false;
        fadeSpeed = speed;
    }

    public void RequestFadeOut(float speed)
    {
        fadeOut = true;
        fadeIn = false;
        fadeSpeed = speed;
    }

    public IEnumerator RequestFadeIn(float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        RequestFadeIn(speed);
    }

    public IEnumerator RequestFadeOut(float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        RequestFadeOut(speed);
    }
    
    private void FadeIn()
    {
        //canvas.sortingOrder = 3;
        canvasGroup.alpha += Time.deltaTime * fadeSpeed;
        if (canvasGroup.alpha >= 1)
        {
            fadeIn = false;
            canvasGroup.alpha = 1;
        }
    }

    private void FadeOut()
    {
        canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.alpha = 0;
            //canvas.sortingOrder = -1;
            fadeOut = false;
        }
    }
    
    public void SetActiveCanvas(bool value)
    {
        canvas.enabled = value;
    }
}
