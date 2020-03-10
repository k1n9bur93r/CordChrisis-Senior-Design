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
    public double tempo;

    // optional
    public double[] note_lengths;
    public int[] note_gesture;
    public double offset;

    #pragma warning restore 0649
}

public class Track : MonoBehaviour
{
    // This is the main class for this file
    // if you want to access members of JsonTrack such as json.notes
    // do so through 'Track.json'
    public string track_file;
    public float note_spacing = 5;
    public float track_width = 20;
    public JsonTrack json;
    public NoteSpawner noteSpawner;
    public GameObject[] note_game_objects;

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

        return track;
    }

    void Start()
    {
        Debug.Log("Note chart reading...");

        // read json track file
        json = readJsonFile(track_file);
        json = validateInput(json);

        // place notes in the game field
        note_game_objects = new GameObject[json.notes.Length];
        for (int i = 0; i < json.notes.Length; i++) {
            int note = json.notes[i];
            double beat = json.beats[i];
            noteSpawner.spawnNote(note - 1, beat);
        }

        Debug.Log("Note chart read!");
    }
}
