﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;

public class PlayerController : MonoBehaviour
{
    private float rayCastMaxRange;

    private Rigidbody rigidBody;

    //private bool canPause;
    private void Awake()
    {
        transform.TryGetComponent(out rigidBody);

        // The maximum distance that the raycast will reach
        rayCastMaxRange = Mathf.Abs(Camera.main.transform.position.y * 1.5f);

        // Create a floor layer in front of the camera to get the mouse position reflection
        CreateFloor();
    }

    private void FixedUpdate()
    {
        Aim();
        Move();
        Pause();
    }

    private void Move()
    {
        // Move player
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        direction = direction.normalized * PlayerAttributes.PLAYER_MOVESPEED * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + direction);
    }

    private void Aim()
    {
        // Create a ray from the mouse cursor on the screen in the direction of the camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform the raycast and see if it hits something on the floorlayer
        if (Physics.Raycast(ray, out RaycastHit hit, rayCastMaxRange, LayerMask.GetMask("Floor")))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = hit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a rotation based on looking down the vector from the player to the mouse.
            //transform.rotation = Quaternion.LookRotation(playerToMouse);
            rigidBody.MoveRotation(Quaternion.LookRotation(playerToMouse));
        }
    }

    private void CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.localScale = new Vector3(5, 1, 3);
        floor.transform.position = new Vector3(0, -1, 0);

        // Make the floor always follow the main camera
        floor.transform.SetParent(Camera.main.transform);

        // Set this object layer to Floor because the ray cast will only hit the floor and ignore other layers
        floor.gameObject.layer = GeneralConst.FLOOR_LAYER;

        // Remove the Mesh Renderer because we only need the Mesh Collider for the ray cast to work
        Destroy(floor.GetComponent<MeshRenderer>());
    }

    private void Pause()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if(Player.main.CheckPauseEnabled())
            UI_InGameMenu_Mechanic.main.UsePause();
        }
    }



    //public void EnablePause()
    //{
        //canPause = true;
    //}

    //public void DisablePause()
    //{
        //canPause = false;
    //}

}