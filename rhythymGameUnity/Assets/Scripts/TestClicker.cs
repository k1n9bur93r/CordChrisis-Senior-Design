using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// THIS IS A TEST FILE TO DEMONSTRATE THE 'Scoreboard' CLASS AND SHOULD NOT BE USED IN FINAL PRODUCTION


public class TestClicker : MonoBehaviour
{
    public Scoreboard sb;
    public double expected_time;
    private float start_time;
    private float end_time;
    // Start is called before the first frame update
    void Start()
    {
        sb = gameObject.GetComponent<Scoreboard>();
        expected_time = 3.0;
    }


    // Update is called once per frame
    private void Update()
    {
        //COPYRIGHTED
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(start_time + "Hold Expected: " + expected_time + "s");
            start_time = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, 100.0f))
            {

                if (hit.transform != null)
                {

                    end_time = Time.time;
                    Debug.Log("Held For: " + (end_time - expected_time) + "s");
                    /*MY CALLS*/

                    string hitName = hit.transform.gameObject.name;
                    switch (hitName)
                    {
                        case "Cube1":
                            sb.UpdateScoreTap(1);
                            break;

                        case "Cube2":
                            sb.UpdateScoreTap(2);
                            break;

                        case "Cube3":
                            sb.UpdateScoreTap(3);
                            break;

                        case "Cube4":
                            sb.UpdateScoreTap(4);
                            break;

                        case "Cube5":
                            sb.UpdateScore(1, expected_time, end_time - start_time);
                            break;

                        case "Cube6":
                            sb.UpdateScore(2, expected_time, end_time - start_time);
                            break;

                        case "Cube7":
                            sb.UpdateScore(3, expected_time, end_time - start_time);
                            break;

                        case "Cube8":
                            sb.UpdateScore(4, expected_time, end_time - start_time);
                            break;


                        default:
                            sb.UpdateScoreTap(0);
                            break;
                    }
                }
            }
            else
            {
                sb.UpdateScoreTap(0);
            }
            /*END MC */
        }
        // END CC
    }





}
