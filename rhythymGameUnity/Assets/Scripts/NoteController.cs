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
     *  GetFirstBeat()
     *  GetSecondBeat()
     *  GetNoteLength()
     */

    public NoteSpawner noteSpawner;

    public double GetNoteLength(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 0)
            return noteSpawner.notes[queueNum][0].GetComponent<NoteMovement>().length;
        else
            return -1.0;
    }

    public double GetFirstBeat(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 0)
        {
            return noteSpawner.notes[queueNum][0].GetComponent<NoteMovement>().beat;
        }

        else
        {
            return double.MaxValue; // Find some other way to deal with this
        }
    }

    public double GetSecondBeat(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 1)
        {
            return noteSpawner.notes[queueNum][1].GetComponent<NoteMovement>().beat;
        }

        else
        {
            return double.MaxValue; // Find some other way to deal with this
        }
    }

    public void RemoveTopNote(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 0)
        {
            noteSpawner.notes[queueNum][0].SetActive(false);
            noteSpawner.notes[queueNum].RemoveAt(0);            
        }
    }
}
