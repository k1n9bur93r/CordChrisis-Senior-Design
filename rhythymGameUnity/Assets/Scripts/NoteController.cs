﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    /*  This class handles the removal of notes from their respective queue.
     *  
     *  Functions:
     *  RemoveNote()
     *      - Deletes the note block at the top of the queue if the player
     *      has pressed within a valid timing window
     *      
     */

    public NoteSpawner noteSpawner;
    public int cutoff;

    public void RemoveTopNote(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 0)
        {
            noteSpawner.notes[queueNum][0].SetActive(false);
            noteSpawner.notes[queueNum].RemoveAt(0);            
        }
    }

    // deprecated
    private bool NoteIsOutOfRange(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 0)
            return (noteSpawner.notes[queueNum][0].GetComponent<NoteMovement>().transform.position.z < cutoff) ? true : false;
        else return false;
    }
}
