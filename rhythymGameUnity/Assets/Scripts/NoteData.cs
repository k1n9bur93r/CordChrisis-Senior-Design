using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    // info created based on NoteCreator
    public double beat;
    public double length;
    public int queueNum;

    void Start()
    {
        // string firstParent = gameObject.transform.parent.name;
        // string secondParent;
        // if (gameObject.transform.parent.transform.parent != null)
        //     secondParent = gameObject.transform.parent.transform.parent.name;
        // else
        //     secondParent = "";

        // if (firstParent == "Note Printer 1" || secondParent == "Note Printer 1")
        //     queueNum = 1;
        // else if (firstParent == "Note Printer 2" || secondParent == "Note Printer 2")
        //     queueNum = 2;
        // else if (firstParent == "Note Printer 3" || secondParent == "Note Printer 3")
        //     queueNum = 3;
        // else if (firstParent == "Note Printer 4" || secondParent == "Note Printer 4")
        //     queueNum = 4;
        // else
        //     queueNum = -1;
    }
}
