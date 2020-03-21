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

    // required
    public double[] beats;
    public int[] notes;
    //public double tempo;

    // optional
    public double[] note_lengths;
    public int[] note_gesture;
    public double offset;
    public double[] tempo_change_amount;
    public double[] tempo_change_beat;

    #pragma warning restore 0649
}

public class Track : MonoBehaviour
{
    // This is the main class for this file
    // if you want to access members of JsonTrack such as json.notes
    // do so through 'Track.json'
    
    public string track_file;
    public JsonTrack json;
    public NoteSpawner noteSpawner;
    public string[] intToGesture = new string[] {"", "l", "r", "u", "d"};

    JsonTrack readJsonFile(string filename) {
        // reads a json file and returns the parsed object as JsonTrack object
        string json_string = Resources.Load<TextAsset>(filename).ToString();
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

        if (track.note_gesture is null) {
            track.note_gesture = new int [length];
        }

        // ---

        if (track.tempo_change_amount.Length != track.tempo_change_beat.Length)
        {
            Debug.Log("tempo changes: " + track.notes.Length + " | tempo beats: " + track.beats.Length);
            throw new System.ArrayTypeMismatchException("Invalid Json file, tempo arrays don't match.");           
        }

        return track;
    }

    void Awake()
    {
        // Read JSON file
        Debug.Log("[Track] Reading...");
        json = readJsonFile(track_file);

        // Validate JSON file
        Debug.Log("[Track] Validating...");
        json = validateInput(json);

        Debug.Log("[Track] Ready!");
    }

    void Start()
    {
        // Spawn the notes
        for (int i = 0; i < json.notes.Length; i++) {
            int note = json.notes[i];
            double beat = json.beats[i];
            int gesture = json.note_gesture[i];
            double length = json.note_lengths[i];
            // string gesture = intToGesture[json.note_gesture[i]];

            if (gesture == 0) {
                // normal note
                noteSpawner.spawnNote(note - 1, beat, length);
            } else {
                // gesture note
                noteSpawner.spawnGesture(gesture - 1, beat);
            }
        }
    }
}
