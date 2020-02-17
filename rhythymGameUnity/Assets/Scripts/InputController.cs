using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Renderer r;
    private Color pressedColor;
    private Color btnColor;
    public KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r + 0.3f, btnColor.g + 0.3f, btnColor.b + 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            r.material.color = pressedColor;
        }
        else if (Input.GetKeyUp(key))
        {
            r.material.color = btnColor;
        }
    }
}
