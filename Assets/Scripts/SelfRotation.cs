using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    [SerializeField, Header("Rotation Maximum Speed")]
    private float maxSpeed;

    [SerializeField, Header("Rotation Acceleration Speed")]
    private float accelerationSpeed;

    [SerializeField, Header("Rotation Speed")]
    private float speed;

    private bool isRotating = true;

    public void SetRotationParameters(bool isRotating = true, float maxSpeed = 500f, float accelerationSpeed = 500f)
    {
        this.isRotating = isRotating;
        this.maxSpeed = maxSpeed;
        this.accelerationSpeed = accelerationSpeed;

    }

    public void SetRotatingStatus(bool isRotating)
    {
        this.isRotating = isRotating;
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            speed += Time.deltaTime * accelerationSpeed;
            if (speed >= maxSpeed) speed = maxSpeed;

            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        }
    }
}
