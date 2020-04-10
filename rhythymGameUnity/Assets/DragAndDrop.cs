using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private GameObject copy;
    private Vector3 mOffset;
    private float mouseZ;
    private float mouseY;
    private float mouseX;

    // Start is called before the first frame update
    void Start()
    {
        copy = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mouseZ;
        //mousePos.y = mouseY;
        //mousePos.x = mouseX;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDown()
    {
        copy = Instantiate(gameObject);
        copy.GetComponent<BoxCollider>().enabled = false;

        mouseZ = Camera.main.WorldToScreenPoint(copy.transform.position).z;
        //mouseY = Camera.main.WorldToScreenPoint(copy.transform.position).y;
        //mouseX = Camera.main.WorldToScreenPoint(copy.transform.position).x;

        mOffset = copy.transform.position - GetMousePos();
    }

    private void OnMouseDrag()
    {
        copy.transform.position = GetMousePos() + mOffset;
    }
}
