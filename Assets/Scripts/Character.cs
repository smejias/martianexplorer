using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float useObjectRange;
    public float speed;
    public float gravity;
    public float jumpForce = 13;
    public CameraController mainCamera;
    public UI ui;
    public float health, currentHealth;
    public GameObject flashlight;
    public AudioClip[] footstepSounds;
    private float _gravityJump = 14;
    private Gun _actualGun;
    private InteractiveObject _interactiveObject;
    private float _runSpeed;
    private Manager _manager;
    private Animator _anim;
    private bool _inmunity;
    private bool _shootingOn = false;
    private bool _canMove = true;
    private bool _isAlive = true;
    private bool _isRunning = false;
    private float _verticalVelocity;
    private float _turnSpeed = 250;
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;
    private float _initialSpeed;
    private bool _undetectable = false;
    private bool _fasterSpeed = false;
    private AudioSource _audioSource;
    private Vector2 _input;
    private float _stepCycle;
    [Range(0f, 1f)] private float _runstepLenghten;
    private float _nextStep;
    private float _stepInterval = 2.5f;
    
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

    public bool Undetectable
    {
        get
        {
            return _undetectable;
        }

        set
        {
            _undetectable = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }

        set
        {
            _isAlive = value;
        }
    }

    public bool ShootingOn
    {
        get
        {
            return _shootingOn;
        }

        set
        {
            _shootingOn = value;
        }
    }

    void Start () {
        _audioSource = GetComponent<AudioSource>();
        _manager = GameObject.Find("GameManager").GetComponent<Manager>();
        _controller = GetComponent<CharacterController>();
        _anim = gameObject.GetComponentInChildren<Animator>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
        _runSpeed = 3;
        currentHealth = health;
        flashlight.SetActive(false);
        _inmunity = false;
        _initialSpeed = speed;
        _stepCycle = 0f;
        _nextStep = _stepCycle / 2f;
    }

	void Update () {
        if (_canMove)
        {
            Shooting();
            Flashlight();
            InteractiveObjectNearby();
            Die();
            UpdateInput();
            Movement();
        }
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            
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
            ShootingOn = true;
            if (Input.GetMouseButton(0))
            {
                GetActualGun();
                ActualGun.Shoot();
            }
        }
        else
        {
            ShootingOn = false;
        }
    }

    private void Movement()
    {
        _anim.SetBool("isGrounded", _controller.isGrounded);
        ProgressStepCycle(speed);
        Run();
        Walk();
        Jump();
    }

    private void GetActualGun()
    {
        ActualGun = (Gun)FindObjectOfType(typeof(Gun));
    }

    public void LandEnd()
    {
        _canMove = true;
    }

    public void LandStart()
    {
        _canMove = false;
    }

    private void Run()
    {
        if (!_fasterSpeed)
        {
            if (Input.GetKey(_manager.utils.run) && _controller.isGrounded)
            {
                speed = _initialSpeed + _runSpeed;
                _isRunning = true;
            }
            else
            {
                speed = _initialSpeed;
                _isRunning = false;
            }
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

        if (ShootingOn)
        {
            ShootingRotation();
        }
        else
        {
            transform.Rotate(0, horizontal * _turnSpeed * Time.deltaTime, 0);
        }

        if (_controller.isGrounded)
        {
            if ((vertical != 0 && !_isRunning) || (horizontal != 0 && !_isRunning))
            {
                _anim.SetInteger("AnimationPar", 1);
            }
            else if (vertical != 0 && _isRunning)
            {
                _anim.SetInteger("AnimationPar", 2);
            }
            else
            {
                _anim.SetInteger("AnimationPar", 0);
            }
        }
    }

    private void ShootingRotation()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100))
        {
            if (!hit.collider.name.Equals(gameObject.name))
            {
                Vector3 playerToMouse = hit.point - transform.position;
                playerToMouse.y = 0f;
                transform.rotation = Quaternion.LookRotation(playerToMouse);
            }
        }
    }

    private void Jump()
    {
        if (_controller.isGrounded)
        {
            _verticalVelocity = -_gravityJump * Time.deltaTime;
            if (Input.GetKeyDown(_manager.utils.jump))
            {
                _anim.SetTrigger("jump");
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

    public void GodMode(bool state)
    {
        GetActualGun();
        if (state)
        {
            ActualGun.GunDamage = 100;
            _inmunity = true;
        }
        else
        {
            ActualGun.GunDamage = ActualGun.InitialGunDamage;
            _inmunity = false;
        }        
    }

    public void FasterMovement(bool state)
    {
        _fasterSpeed = state;
        if (state && speed < 15)
        {
            speed += 15;
        }
        else
        {
            speed = _initialSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_inmunity) {
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
            _canMove = false;
            _isAlive = false;
            _anim.SetTrigger("Die");
        }
    }

    public void Undetect(bool state)
    {
        Undetectable = state;
    }

    private void ProgressStepCycle(float speed)
    {
        if ((_input.x != 0 || _input.y != 0) && _controller.isGrounded)
        {
            _stepCycle += (_controller.velocity.magnitude + speed) * Time.deltaTime;
        }
        if (!(_stepCycle > _nextStep))
        {
            return;
        }
        _nextStep = _stepCycle + _stepInterval;

        PlayFootStepAudio();
    }

    public void UpdateInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _input = new Vector2(horizontal, vertical);
        if (_input.sqrMagnitude > 1)
        {
            _input.Normalize();
        }
    }

    private void PlayFootStepAudio()
    {
        int number = UnityEngine.Random.Range(1, footstepSounds.Length);
        _audioSource.clip = footstepSounds[number];
        _audioSource.PlayOneShot(_audioSource.clip);
        footstepSounds[number] = footstepSounds[0];
        footstepSounds[0] = _audioSource.clip;
    }
}
