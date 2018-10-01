using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public AudioClip gunAudio;
    public Transform gunEnd;
    public CameraController mainCamera;
    private int _gunDamage;
    protected float fireRate;
    protected float weaponRange;
    protected float hitForce;               
    protected WaitForSeconds shotDuration = new WaitForSeconds(0.07f);  
    protected LineRenderer laserLine;
    protected float nextFire;
    protected Manager manager;
    protected int _initialGunDamage;
    protected AudioSource _audioSource;
    protected Character actualPlayer;

    public int GunDamage
    {
        get
        {
            return _gunDamage;
        }

        set
        {
            _gunDamage = value;
        }
    }

    public int InitialGunDamage
    {
        get
        {
            return _initialGunDamage;
        }

        set
        {
            _initialGunDamage = value;
        }
    }

    void Start ()
    {        
        mainCamera = GameObject.Find("MainCamera").GetComponent<CameraController>();
    }

    void Update () {
	}

    public virtual void Shoot()
    {
        if (Time.time > nextFire && !manager.Paused)
        {
            nextFire = Time.time + fireRate;
            Vector3 rayOrigin = transform.position;
            RaycastHit hit;
            laserLine.SetPosition(0, gunEnd.position);
            Rect rect = new Rect (Input.mousePosition.x, Input.mousePosition.y, 0, 0);
			Ray mouseDirection = Camera.main.ScreenPointToRay (rect.center);
			Vector3 pointRay = Vector3.zero;

			if (Physics.Raycast (mouseDirection, out hit, 1000)) 
            {
                pointRay = hit.point;
                Vector3 direction = (pointRay - transform.position).normalized;
                if (Physics.Raycast(rayOrigin,direction, out hit, weaponRange))
                {
                    laserLine.SetPosition(1, hit.point);

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * hitForce);
                        hit.transform.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
                    }
                }
                else
                {
                    laserLine.SetPosition(1, rayOrigin + (direction * weaponRange));                
                }
                StartCoroutine(ShotEffect());
			}
        }
    }

    private IEnumerator ShotEffect()
    {
        if (gunAudio != null)
        {
            _audioSource.PlayOneShot(gunAudio);
        }
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

    protected void FindPlayer()
    {
        if (actualPlayer == null)
        {
            actualPlayer = (Character)FindObjectOfType(typeof(Character));
        }
    }

    protected void SetGunDamage(int damageReducer)
    {
        GunDamage = (int)actualPlayer.health / damageReducer;
        _initialGunDamage = GunDamage;
    }
}
