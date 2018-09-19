using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Obstacle {

    public bool switchOn;
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    ParticleSystem system
    {
        get
        {
            if (_CachedSystem == null)
                _CachedSystem = GetComponent<ParticleSystem>();
            return _CachedSystem;
        }
    }
    private ParticleSystem _CachedSystem;

    public Rect windowRect = new Rect(0, 0, 300, 120);

    public bool includeChildren = true;

    void OnGUI()
    {
        windowRect = GUI.Window("ParticleController".GetHashCode(), windowRect, DrawWindowContents, system.name);
    }

    void DrawWindowContents(int windowId)
    {
        if (system)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Toggle(system.isPlaying, "Playing");
            GUILayout.Toggle(system.isEmitting, "Emitting");
            GUILayout.Toggle(system.isPaused, "Paused");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
                system.Play(includeChildren);
            if (GUILayout.Button("Pause"))
                system.Pause(includeChildren);
            if (GUILayout.Button("Stop Emitting"))
                system.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
            if (GUILayout.Button("Stop & Clear"))
                system.Stop(includeChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
            GUILayout.EndHorizontal();

            includeChildren = GUILayout.Toggle(includeChildren, "Include Children");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Time(" + system.time + ")");
            GUILayout.Label("Particle Count(" + system.particleCount + ")");
            GUILayout.EndHorizontal();
        }
        else
            GUILayout.Label("No particle system found");

        GUI.DragWindow();
    }

    void Update () {
        Switch();
    }

    public void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.SendMessageUpwards("TakeDamage", damage);
        }
    }

    public void Switch()
    {
        if (ps.isPlaying && switchOn == false)
        {
            ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
        }

    }
}
