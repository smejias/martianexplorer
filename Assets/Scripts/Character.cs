using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private Animator _anim;
    public float speed;
    public float gravity;
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;
    private float _verticalVelocity;
    private float _turnSpeed = 250;
    public float jumpForce = 13;
    private float _gravityJump = 14;
    private Gun _actualGun;
    public CameraController mainCamera;
    private float _runSpeed;
    public Manager manager;
    public float health, currentHealth;
    public GameObject flashlight;
    private InteractableObject _interactableObject;
    public float useObjectRange;
    private bool inmunity;

    void Start () {
        _controller = GetComponent<CharacterController>();
        _anim = gameObject.GetComponentInChildren<Animator>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        _runSpeed = speed + 5;
        currentHealth = health;
        flashlight.SetActive(false);
        inmunity = false;
    }

	void Update () {
        Movement();
        Shooting();
        Flashlight();
        InteractableObjectNearby();
    }

    private void InteractWithObject(GameObject interactableObject)
    {
        _interactableObject = interactableObject.GetComponent<InteractableObject>();
        _interactableObject.Activate(transform.position);
    }

    private void InteractableObjectNearby()
    {
        if (mainCamera.MousePosition() != null)
        {
            GameObject interactableObject = mainCamera.MousePosition();

            if (interactableObject.tag.Equals("InteractableObject") &&
                Input.GetKeyDown(manager.utils.interactWithObjects) &&
                Vector3.Distance(interactableObject.transform.position, transform.position) < useObjectRange)
            {
                InteractWithObject(interactableObject);
            }
        }
    }

    private void Shooting()
    {
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            _actualGun = (Gun) FindObjectOfType(typeof(Gun));
            _actualGun.Shoot();
        }
    }

    private void Movement()
    {
        Run();
        Walk();
        Jump();
    }

    private void Run()
    {
        if (Input.GetKey(manager.utils.run))
        {
            speed = _runSpeed;            
        }
        else
        {
            speed = _runSpeed - 5;
        }
    }

    private void Walk()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(0, 0, vertical);
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= speed;

        _moveDirection.y = gravity * Time.deltaTime * Physics.gravity.y;
        _controller.Move(_moveDirection * Time.deltaTime);      

        if (CheckFPS())
        {
            FpsRotation();
        }
        else
        {
            transform.Rotate(0, horizontal * _turnSpeed * Time.deltaTime, 0);
        }

        if (vertical != 0 && _controller.isGrounded)
        {
            _anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            _anim.SetInteger("AnimationPar", 0);
        }
    }

    private void FpsRotation()
    {
        transform.forward = new Vector3(mainCamera.transform.forward.x, transform.forward.y, mainCamera.transform.forward.z);
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {          
            _verticalVelocity = -_gravityJump * Time.deltaTime;
            if (Input.GetKeyDown(manager.utils.jump))
            {
                _verticalVelocity = jumpForce;
            }
        }
        else
        {
            _verticalVelocity -= _gravityJump * Time.deltaTime;
        }
        Vector3 jumpVector = new Vector3(0, _verticalVelocity, 0);
        _controller.Move(jumpVector * Time.deltaTime);
    }

    public bool CheckFPS()
    {
        return mainCamera.fpsOn;
    }

    public void GodMode()
    {
        jumpForce += 50;

        inmunity = !inmunity;
        //TO DO - Lot of damage
    }

    public void TakeDamage(float damage)
    {
        if (!inmunity) {
            currentHealth -= damage;
        }
    }

    public void Flashlight()
    {
        if (Input.GetKeyDown(manager.utils.flashlight))
        {
            flashlight.SetActive(!flashlight.activeInHierarchy);
        }
    }
}
