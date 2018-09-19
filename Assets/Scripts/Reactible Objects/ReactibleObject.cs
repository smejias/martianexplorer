using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactibleObject : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    public void TurnOffOn(bool state)
    {
        gameObject.SetActive(state);
    }

    public virtual void Trigger()
    {

    }
}