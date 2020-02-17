using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Renderer r;
    public KeyCode key;
    private Color pressedColor;
    private Color btnColor;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        btnColor = r.material.color;
        pressedColor = new Color(btnColor.r, btnColor.g + 0.4f, btnColor.b + 0.4f);
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
