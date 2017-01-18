using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem[] systems;


    public void Start()
    {
        systems = GetComponentsInChildren<ParticleSystem>();
    }

    public void Update()
    {
        if(systems.Length > 0)
        {
            foreach (ParticleSystem system in systems)
            {
                if(system.IsAlive())
                    return;

            }

            Destroy(gameObject);
        }
    }
}