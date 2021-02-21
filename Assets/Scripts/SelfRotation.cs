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

    public void SetRotationParameters(float _maxSpeed = 500f, float _accelerationSpeed = 500f)
    {
        maxSpeed = _maxSpeed;
        accelerationSpeed = _accelerationSpeed;
    }
    private void FixedUpdate()
    {
        speed += Time.deltaTime * accelerationSpeed;
        if (speed >= maxSpeed) speed = maxSpeed;

        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
