using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputController : MonoBehaviour
{
    public float distance = 50f;

    public List<float> distances;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // RaycastHit  hit;
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // if (Physics.Raycast(ray, out hit)) {
            //     print(hit.transform.name);
            //     if (hit.transform.name == "Red_Receptor" )
            //     {print( "My object is clicked by mouse");}
            // }

                          //create a ray cast and set it to the mouses cursor position in game

            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, distance)) 
            {
                //draw invisible ray cast/vector
                Debug.DrawLine (ray.origin, hit.point);
                //log hit area to the console
                Debug.Log(hit.point);

                if (hit.point.x < distances[0])
                {
                    print("button 1");
                }
                else if (hit.point.x < distances[1])
                {
                    print("button 2");
                }
                else if (hit.point.x < distances[2])
                {
                    print("button 3");
                }
                else
                {
                    print("button 4");
                }  
            }    

        }
    }
}
