using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTest : MonoBehaviour
{
    public Material mainColor;
    public Material whiteEmission;
    public float dissolveLevel = 1.1f;
    public float dissolveRate = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        // Make the note block transparent
        mainColor.SetFloat("DEffect", dissolveLevel);
        whiteEmission.SetFloat("DEffect", dissolveLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dissolveLevel = 1.0f;
        }
        if(dissolveLevel >= 0.0f && dissolveLevel <= 1.0f)
        {
            dissolveLevel = dissolveLevel - (dissolveRate * Time.deltaTime);
            mainColor.SetFloat("DEffect", dissolveLevel);
            whiteEmission.SetFloat("DEffect", dissolveLevel);
        }
    }
}
