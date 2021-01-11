using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfMovingFoward : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 direction;
    void Start()
    {
        //direction = 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 10);
    }
}
