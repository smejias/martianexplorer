using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Obstacle {

    public bool switchOn;
    private ParticleSystem _ps;

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
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

    void Update () {
        Switch();
    }

    public void OnParticleCollision(GameObject other)
    {
        DoDamage(other, damage);
    }

    public void Switch()
    {
        if (_ps.isPlaying && switchOn == false)
        {
            _ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
