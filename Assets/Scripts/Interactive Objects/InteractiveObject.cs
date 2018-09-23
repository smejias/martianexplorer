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

    public virtual void Hit()
    {
        if (ps != null)
        {
            ps.GetComponent<Fire>().switchOn = false;
        }

        FindObjectOfType<AudioManager>().Play("Transformer Down");
        GetComponent<Renderer>().material = offMaterial;
        GetComponent<Renderer>().material = lightOn;
    }

    public void TriggerObject()
    {
        if (reactibleObject != null)
        {
            reactibleObject.Trigger();
        }
    }
}
