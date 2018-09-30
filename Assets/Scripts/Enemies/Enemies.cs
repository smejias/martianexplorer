using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    public float life;
    public GameObject target;
    public WP waypoints;
    public TextMesh damageText;
    protected float speed;  
    protected int minSpotedDistance;
    protected int maxSpotedDistance;    
    protected float pauseDuration = 1;
    protected float attackRate = 2;
    protected float dampingLook = 3;
    protected bool loop = true;    
    protected float minAttackDistance;    
    protected float animationDieLength;
    protected float damage;
    protected float nextAttack;
    protected bool _undetecting;
    protected Character _actualPlayer;
    private int _currentWp;
    private float curTime;
    private bool advance = true;
    private bool canMove = true;
    private bool canAttack = true;
    private bool playerSpoted = false;
    private MeshRenderer _renderer;
    private Color _normalColor;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _normalColor = _renderer.material.color;
        damage = 5;
        speed = 5;
        minSpotedDistance = 7;
        maxSpotedDistance = 15;
        minAttackDistance = 2.5f;
        animationDieLength = 1;
        Player();
        FindTarget();
        SetEnemyLife();
    }

    void Update()
    {
        SpotTarget();
        Move();
        Attack();        
        Die();
    }

    private void FindTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player") as GameObject;
        }
    }

    public void MoveToWaypoints()
    {
        Vector3 currentTarget = waypoints.waypoints[_currentWp].position;
        currentTarget.y = transform.position.y;
        Vector3 moveDirection = currentTarget - transform.position;

        if (moveDirection.magnitude < 0.5f)
        {
            if (curTime == 0)
                curTime = Time.time;
            if ((Time.time - curTime) >= pauseDuration)
            {
                if (advance)
                {
                    _currentWp++;
                }
                else
                {
                    _currentWp--;
                }
                curTime = 0;
            }
        }
        else
        {
            var rotation = Quaternion.LookRotation(currentTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
            transform.position += moveDirection.normalized * speed * Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (canAttack && Vector3.Distance(transform.position, target.transform.position) <= minAttackDistance)
        {
            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                HitPlayer();
            }
        }
    }

    private void HitPlayer()
    {
        if (_actualPlayer.IsAlive)
        {
            target.SendMessageUpwards("TakeDamage", damage);
        }
    }

    public void Move()
    {
        if (canMove)
        {
            if (!playerSpoted || _actualPlayer.Undetectable)
            {
                if (waypoints != null && waypoints.waypoints.Length > 0)
                {
                    if (_currentWp < waypoints.waypoints.Length && _currentWp >= 0)
                    {
                        MoveToWaypoints();
                    }
                    else
                    {
                        if (loop)
                        {
                            if (_currentWp <= 0)
                            {
                                _currentWp++;
                                advance = true;
                            }
                            else
                            {
                                _currentWp--;
                                advance = false;
                            }
                            MoveToWaypoints();
                        }
                    }
                }
            }
            else
            {
                GoTo();
            }
        }
    }

    public void GoTo()
    {
        transform.LookAt(target.transform);

        if (Vector3.Distance(transform.position, target.transform.position) >= minAttackDistance)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    public void SpotTarget()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        Vector3 targetDir = target.transform.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        if (distance < minSpotedDistance || (angleToPlayer >= -90 && angleToPlayer <= 90 && distance < maxSpotedDistance))
        {
            playerSpoted = true;
        }
    }

    public void Hit()
    {
        TakeDamage(_actualPlayer.ActualGun.GunDamage);
        if (!_actualPlayer.Undetectable)
        {
            playerSpoted = true;
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        DamageFlash();
        FloatingText(damage);
    }

    private void FloatingText(float damage)
    {
        TextMesh _damageText = Instantiate(damageText, transform.position, transform.rotation);
        _damageText.text = "-" + damage;
    }

    public void Die()
    {
        if (life <= 0)
        {
            Invoke("Destroy", animationDieLength);
            canMove = false;
            canAttack = false;
        }
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void Player()
    {
        _actualPlayer = (Character)FindObjectOfType(typeof(Character));
    }

    private void DamageFlash()
    {
        _renderer.material.color = Color.red;
        Invoke("ResetColor", 0.2f);
    }

    private void ResetColor()
    {
        _renderer.material.color = _normalColor;
    }

    public void SetEnemyLife()
    {
        life = (int)_actualPlayer.health / 5;
    }
}