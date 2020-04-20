using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTrack
{
    // Defines the format for the Json Serializer

    // Disable warnings of the form:
    // 'JsonTrack.notes' is never assigned to, and will always have its default value null
    // It gets assigned to in the json serializer

    #pragma warning disable 0649

    // Song metadata
    public AudioClip audio;
    public string background;
    public string title;
    public string artist;

    // Chart pieces
    public double offset;
    public double[] beats;
    public int[] notes;
    public double[] note_lengths;
    public double tempo_normal;
    public double[] tempo_change_amount;
    public double[] tempo_change_beat;

    #pragma warning restore 0649
}

public class Track : MonoBehaviour
{
    // This is the main class for this file
    // if you want to access members of JsonTrack such as json.notes
    // do so through 'Track.json'

    // Track vars
    [Header("Used by SiteHandler - LEAVE THIS BLANK")]
    public string track_file;
    public JsonTrack json;
    public NoteSpawner noteSpawner;
    public string[] intToGesture = new string[] {"", "l", "r", "u", "d"};

    // Derived chart statistics
    public int noteTotal;

    JsonTrack readJsonFile() {
        // reads a json file and returns the parsed object as JsonTrack object
        //string json_string = track_file; //Resources.Load<TextAsset>(filename).ToString();

		GameObject files = GameObject.Find("SiteHandler");
        string json_string;

		if (files.GetComponent<SiteHandler>().webMode)
		{
            json_string = files.GetComponent<SiteHandler>().chartFile;
		}

		else
		{
			json_string = Resources.Load<TextAsset>(files.GetComponent<SiteHandler>().chartLocation).ToString();
		}

        JsonTrack json = JsonUtility.FromJson<JsonTrack>(json_string);

        return json;
    }

    JsonTrack validateInput(JsonTrack track) {
        // variables in c# are automatically initialized,
        // so they are not explicitly initialized here

        int length = track.notes.Length;

        if (track.notes.Length != track.beats.Length) {
            Debug.Log("notes: " + track.notes.Length + " | beats: " + track.beats.Length);
            throw new System.ArrayTypeMismatchException("Invalid Json file, notes and beats length don't match.");
        }

        if (track.note_lengths is null) {
            track.note_lengths = new double [length];
        }

        if (track.notes.Length != track.note_lengths.Length) {
            Debug.Log("notes: " + track.notes.Length + " | note_lengths: " + track.note_lengths.Length);
            throw new System.ArrayTypeMismatchException("Invalid Json file, notes and note_lengths length don't match.");
        }

        // ---

        if (track.tempo_change_amount.Length != track.tempo_change_beat.Length)
        {
            Debug.Log("tempo changes: " + track.notes.Length + " | tempo beats: " + track.beats.Length);
            throw new System.ArrayTypeMismatchException("Invalid Json file, tempo arrays don't match.");
        }

        return track;
    }

    private int CalculateNoteTotal()
    {
        int temp = json.notes.Length;

        for (int i = 0; i < json.notes.Length; i++)
        {
            if (json.note_lengths[i] > 0)
            {
                temp++;
            }
        }

        return temp;
    }

    void Awake()
    {
        // Read JSON file
        Debug.Log("[Track] Reading...");
        json = readJsonFile();

        // Validate JSON file
        Debug.Log("[Track] Validating...");
        json = validateInput(json);

        Debug.Log("[Track] Ready!");
    }

    void Start()
    {
        // Spawn the notes
        for (int i = 0; i < json.notes.Length; i++) {
            //Debug.Log(json.beats[i]); // !
            int note = json.notes[i];
            double beat = json.beats[i];
            double length = json.note_lengths[i];

            if (1 <= note && note <= 4) {
                // normal note
                noteSpawner.spawnNote(note - 1, beat, length);
            } else if (5 <= note && note <= 8) {
                // gesture note
                noteSpawner.spawnGesture(note - 5, beat);
            } else {
                Debug.Log("[Track] Invalid note found: " + note);
            }
        }

        // Calculate chart statistics
        noteTotal = CalculateNoteTotal();
    }
}
