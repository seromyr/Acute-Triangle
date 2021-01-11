using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// PUT THIS ON THE JOYSTICK HANDLE
public class UI_JoystickHandle_Reader : MonoBehaviour
{
    [SerializeField]
    private Color color1, color2;

    [SerializeField, Range(0f, 10f)]
    private float lerpSpeed;

    private RectTransform rectTransform;
    private Image image;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (rectTransform.localPosition != Vector3.zero)
        {
            image.color = Color.Lerp(image.color, color2, Time.deltaTime * lerpSpeed);
        }
        else
        {
            image.color = Color.Lerp(image.color, color1, Time.deltaTime * lerpSpeed);
        }
    }
}
