using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptorParticle : MonoBehaviour
{
    private static bool splash = false;
    public ParticleSystem particleLauncher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            splash = true;
        }
        if (splash)
        {
            particleLauncher.Emit(5);
            splash = false;
        }
    }
}
