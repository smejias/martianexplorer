using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    protected int gunDamage;
    protected float fireRate;
    protected float weaponRange;
    protected float hitForce;                                       
    public Transform gunEnd;
    public Camera mainCamera;                                              
    protected WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    
    protected AudioSource gunAudio;                                       
    protected LineRenderer laserLine;
    protected float nextFire;
    protected Manager manager;

    void Start () {
    }
	
	void Update () {
	}

    public virtual void Shoot()
    {
        if (Time.time > nextFire && !manager.Paused)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            laserLine.SetPosition(0, gunEnd.position);
            FindObjectOfType<AudioManager>().Play("Laser Shot");

            if (Physics.Raycast(rayOrigin, mainCamera.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                    hit.transform.SendMessage("Hit");
                    Debug.Log("Hit something");
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (mainCamera.transform.forward * weaponRange));
                
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        //gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
