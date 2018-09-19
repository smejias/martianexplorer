using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

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

    void Hit()
    {
        if (ps != null)
        {
            ps.GetComponent<Fire>().switchOn = false;
        }

        FindObjectOfType<AudioManager>().Play("Transformer Down");
        GetComponent<Renderer>().material = offMaterial;
        GetComponent<Renderer>().material = lightOn;
    }

    public virtual void Activate(Vector3 playerPosition)
    {
        if (Vector3.Distance(transform.position, playerPosition) < 3f)
        {
            GetComponent<Renderer>().material = lightOn;
            if (door != null)
            {
                door.TurnOffOn(false);
            }

            TriggerObject();
        }
        else
        {
            GetComponent<Renderer>().material = offMaterial;
        }
    }

    public void TriggerObject()
    {
        if (reactibleObject != null)
        {
            reactibleObject.Trigger();
        }
    }
}
