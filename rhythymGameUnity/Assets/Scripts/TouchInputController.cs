using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputController : MonoBehaviour
{
    public float distance = 50f;
    public List<float> distances;
    public List<bool> held;
    public List<bool> touched;
    // Start is called before the first frame update
    void Start()
    {
        for (int x=0; x<4; x++)
        {
            touched.Add(false);
            held.Add(false);
        }
    }

    // Update is called once per frame
    public void checkTouch()
    {
       for (int x = 0; x < Input.touchCount; x++)
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(x).position);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, distance)) 
            {
                //draw invisible ray cast/vector
                Debug.DrawLine (ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);

                for (int y = 0; y < 4; y++)
                {
                    if (hit.point.x < distances[y])
                    {
                        print("button " + (y+1));

                        if (Input.GetTouch(x).phase == TouchPhase.Began)
                            touched[y]=true;

                        held[y]=true;

                        break;
                    }
                }
            }    
        }
    }
    
}