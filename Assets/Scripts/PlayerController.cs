using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Joystick joystick;

    [SerializeField]
    private Transform playerPointer;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float speedMultiplier;

    void Start()
    {

    }

    public void LoadJoystick()
    {
        joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
    }

    private void FixedUpdate()
    {
        if (joystick != null) ControllerEngage();
    }

    private void ControllerEngage()
    {
        speed = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude * speedMultiplier;

        if (joystick.Horizontal > 0 || joystick.Horizontal < 0 || joystick.Vertical > 0 || joystick.Vertical < 0)
        {
            playerPointer.position = new Vector3(joystick.Horizontal + transform.position.x, playerPointer.position.y, joystick.Vertical + transform.position.z);

            transform.LookAt(new Vector3(playerPointer.position.x, 0, playerPointer.position.z));

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        transform.Translate(transform.InverseTransformDirection(Vector3.forward) * Time.deltaTime * speedMultiplier * Input.GetAxis("Vertical"));
        transform.Translate(transform.InverseTransformDirection(Vector3.right) * Time.deltaTime * speedMultiplier * Input.GetAxis("Horizontal"));
    }
}
