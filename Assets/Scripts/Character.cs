﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private Animator anim;
    public float speed;
    public float gravity;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float verticalVelocity;
    private float turnSpeed = 150;
    public float jumpForce = 13;
    private float gravityJump = 14;
    private Gun actualGun;
    
    void Start () {
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
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
            actualGun = (Gun) FindObjectOfType(typeof(Gun));
            actualGun.Shoot();
        }
    }

    private Gun GetActualGun()
    {
        return actualGun;
    }

    private void Movement()
    {
        Walk();
        Run();
        Jump();
    }

    private void Run()
    {
        //throw new NotImplementedException();
    }

    private void Walk()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        moveDirection.y = gravity * Time.deltaTime * Physics.gravity.y;
        controller.Move(moveDirection * Time.deltaTime);

        transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);

        if (vertical != 0 && controller.isGrounded)
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
        }
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
