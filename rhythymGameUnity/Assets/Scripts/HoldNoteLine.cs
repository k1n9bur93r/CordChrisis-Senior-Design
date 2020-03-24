using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteLine : MonoBehaviour
{
    public GameObject firstNote;
    public GameObject secondNote;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        firstNote = gameObject;
        line = gameObject.GetComponent<LineRenderer>();
        line.startWidth = 0f;
        line.endWidth = 0f;
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstNote != null && secondNote != null)
        {
            line.SetPosition(0, firstNote.transform.position);
            line.SetPosition(1, secondNote.transform.position);

            if (!secondNote.activeSelf)
            {
                print("WOW");
                line.SetPosition(1, new Vector3(line.GetPosition(1).x,line.GetPosition(1).y,1));
            }
        }
    }
}
