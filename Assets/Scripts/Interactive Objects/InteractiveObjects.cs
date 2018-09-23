using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

    public Door door;
    public GameObject ps;
    public Material offMaterial, lightOn;
    public ReactibleObject reactibleObject;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public virtual void Activate(Vector3 playerPosition)
    {
            GetComponent<Renderer>().material = lightOn;
            if (door != null)
            {
                door.TurnOffOn(false);
            }
            TriggerObject();            
    }

    public void TriggerObject()
    {
        if (reactibleObject != null)
        {
            reactibleObject.Trigger();
        }
    }
}
