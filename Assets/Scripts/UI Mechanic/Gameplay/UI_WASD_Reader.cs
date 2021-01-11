using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WASD_Reader : MonoBehaviour
{
    [SerializeField]
    private List<Image> actionKeys;

    [SerializeField]
    private Color color1, color2;

    [SerializeField, Range(0f, 10f)]
    private float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        actionKeys.AddRange(GetComponentsInChildren<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        ChangeKeyColor(KeyCode.W, 0);
        ChangeKeyColor(KeyCode.A, 1);
        ChangeKeyColor(KeyCode.S, 2);
        ChangeKeyColor(KeyCode.D, 3);
    }

    private bool CheckKeyPressed(KeyCode key)
    {
        return Input.GetKey(key);
    }

    private void ChangeKeyColor(KeyCode key, int keyIndex)
    {
        if (CheckKeyPressed(key))
        {
            actionKeys[keyIndex].color = Color.Lerp(actionKeys[keyIndex].color, color2, Time.deltaTime * lerpSpeed);
        }
        else
        {
            actionKeys[keyIndex].color = Color.Lerp(actionKeys[keyIndex].color, color1, Time.deltaTime * lerpSpeed);
        }
    }
}
