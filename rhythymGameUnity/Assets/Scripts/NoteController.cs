using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public Metronome metronome;
    public NoteSpawner noteSpawner;
    public int cutoff;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (NoteIsOutOfRange(i))
                RemoveTopNote(i);
        }
    }

    private void RemoveTopNote(int queueNum)
    {
        noteSpawner.notes[queueNum][0].SetActive(false);

        if (!(noteSpawner.notes[queueNum].Count < 0))
            noteSpawner.notes[queueNum].RemoveAt(0);
    }

    private bool NoteIsOutOfRange(int queueNum)
    {
        if (noteSpawner.notes[queueNum].Count > 0)
            return (noteSpawner.notes[queueNum][0].GetComponent<NoteMovement>().transform.position.z < cutoff) ? true : false;
        else return false;
    }
}
