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

    public void SetRotationParameters(float _maxSpeed = 500f, float _accelerationSpeed = 500)
    {
        maxSpeed = _maxSpeed;
        accelerationSpeed = _accelerationSpeed;
    }

    public void Start()
    {
        //maxSpeed = 50;
        //speed = 0;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        speed += Time.deltaTime * accelerationSpeed;
        if (speed >= maxSpeed) speed = maxSpeed;

        transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
