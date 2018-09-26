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
    public UI ui;
    private float _runSpeed;
    private Manager _manager;
    public float health, currentHealth;
    public GameObject flashlight;
    private InteractiveObject _interactiveObject;
    public float useObjectRange;
    private bool inmunity;
    private bool fpsOn = false;
    private bool canMove = true;
    private bool isAlive = true;

    public Gun ActualGun
    {
        get
        {
            return _actualGun;
        }

        set
        {
            _actualGun = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        set
        {
            isAlive = value;
        }
    }

    private void Awake()
    {    
        //ui = GameObject.Find("Canvas").GetComponent<UI>();
    }

    void Start () {
        _manager = GameObject.Find("GameManager").GetComponent<Manager>();
        _controller = GetComponent<CharacterController>();
        _anim = gameObject.GetComponentInChildren<Animator>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        _runSpeed = speed + 5;
        currentHealth = health;
        flashlight.SetActive(false);
        inmunity = false;
    }

	void Update () {
        if (canMove)
        {
            Movement();
            Shooting();
            Flashlight();
            InteractiveObjectNearby();
            Die();
        }
    }

    private void InteractWithObject(GameObject interactableObject)
    {
        _interactiveObject = interactableObject.GetComponent<InteractiveObject>();
        _interactiveObject.Activate(transform.position);
    }

    private void InteractiveObjectNearby()
    {
        if (mainCamera.MousePosition() != null)
        {
            GameObject interactableObject = mainCamera.MousePosition();

            if (interactableObject.tag.Equals("InteractiveObject") &&
                Input.GetKeyDown(_manager.utils.interactWithObjects) &&
                Vector3.Distance(interactableObject.transform.position, transform.position) < useObjectRange)
            {
                InteractWithObject(interactableObject);
            }
        }
    }

    private void Shooting()
    {
        if (Input.GetMouseButton(1) && !_manager.Paused)
        {
            fpsOn = true;
            if (Input.GetMouseButton(0))
            {
                ActualGun = (Gun)FindObjectOfType(typeof(Gun));
                ActualGun.Shoot();
            }
        }
        else
        {
            fpsOn = false;
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
        if (Input.GetKey(_manager.utils.run))
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

        _moveDirection = new Vector3(horizontal, 0, vertical);
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= speed;

        _moveDirection.y = gravity * Time.deltaTime * Physics.gravity.y;
        _controller.Move(_moveDirection * Time.deltaTime);      

        if (fpsOn)
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
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 100))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            transform.rotation = Quaternion.LookRotation(playerToMouse);
        }
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {          
            _verticalVelocity = -_gravityJump * Time.deltaTime;
            if (Input.GetKeyDown(_manager.utils.jump))
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
        if (Input.GetKeyDown(_manager.utils.flashlight))
        {
            flashlight.SetActive(!flashlight.activeInHierarchy);
        }
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            canMove = false;
            isAlive = false;
        }
    }
}
