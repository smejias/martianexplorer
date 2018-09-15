using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float speed;
    public float gravity;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float verticalVelocity;
    public float jumpForce = 13;
    private float gravityJump = 14;
    
    void Start () {
        controller = GetComponent<CharacterController>();
    }

	void Update () {
        Movement();
        Shooting();
        InteractWithObject();
	}

    private void InteractWithObject()
    {
        if (Input.GetKeyDown(KeyCode.E) && InteractableObjectNearby())
        {
            //TO DO - Action when interact
        }
    }

    private bool InteractableObjectNearby()
    {
        bool interactable = false;

        //TO DO - Find interactable objects and check the distance

        return interactable;
    }

    private void Shooting()
    {
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            //TO DO - Shoot and bullet firing
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        moveDirection.y = gravity * Time.deltaTime * Physics.gravity.y;
        controller.Move(moveDirection * Time.deltaTime);

        transform.Rotate(0, horizontal, 0);

        Jump();
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -gravityJump * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravityJump * Time.deltaTime;
        }

        Vector3 jumpVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(jumpVector * Time.deltaTime);
    }
}
