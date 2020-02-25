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
    public double[] beats;
    public int[] notes;
    public double tempo;
    public double offset;
    #pragma warning restore 0649
}

public class Track : MonoBehaviour
{
    // This is the main class for this file
    // if you want to access members of JsonTrack such as json.notes
    // do so through 'Track.json'
    public string track_file = "Text/more";
    public float note_spacing = 5;
    public float track_width = 20;
    public JsonTrack json;
    public GameObject[] note_game_objects;

    GameObject createNote(Vector3 position) {
        // creates a note at the given position
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        return cube;
    }

    JsonTrack readJsonFile(string filename) {
        // reads a json file and returns the parsed object as JsonTrack object
        string json_string = Resources.Load<TextAsset>(filename).ToString();
        JsonTrack json = JsonUtility.FromJson<JsonTrack>(json_string);
        return json;
    }

    void Start()
    {
        // read json track file
        json = readJsonFile(track_file);

        if (json.notes.Length != json.beats.Length) {
            throw new System.ArrayTypeMismatchException("Invalid Json file, notes and beats length don't match.");
        }

        int max_note = 0;
        foreach (int note in json.notes) {
            max_note = (max_note > note) ? (max_note) : (note);
        }
        float note_width = track_width / max_note;

        // place notes in the game field
        note_game_objects = new GameObject[json.notes.Length];
        for (int i = 0; i < json.notes.Length; i++) {
            int note = json.notes[i];
            double beat = json.beats[i];
            Vector3 position = new Vector3((note - 1) * note_width - (track_width - note_width) / 2, -1, ((float) beat) * note_spacing);
            note_game_objects[i] = createNote(position);
        }
    }
}
