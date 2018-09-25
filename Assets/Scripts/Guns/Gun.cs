using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    private int _gunDamage;
    protected float fireRate;
    protected float weaponRange;
    protected float hitForce;                                       
    public Transform gunEnd;
    public CameraController mainCamera;                                              
    protected WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    
    protected AudioSource gunAudio;                                       
    protected LineRenderer laserLine;
    protected float nextFire;
    protected Manager manager;

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

    void Start () {
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
            FindObjectOfType<AudioManager>().Play("Laser Shot");
			Ray mouseDirection = Camera.main.ScreenPointToRay (Input.mousePosition);
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
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
