using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    private float speed = 2;
    private CameraController _cameraToLookAt;

    void Start () {
        Invoke("Destroy", 2);
        _cameraToLookAt = GameObject.Find("MainCamera").GetComponent<CameraController>();
    }
	
	void Update () {
        Move();
        Rotate();
	}

    private void Rotate()
    {
        Vector3 v = _cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(_cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
    }

    private void Move()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
