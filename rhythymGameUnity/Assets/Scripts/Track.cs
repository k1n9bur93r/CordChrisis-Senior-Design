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
    public string track_file = "Text/more";
    public float note_spacing = 5;
    public float note_width = 5;
    public JsonTrack json;

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

        // place notes in the game field
        for (int i = 0; i < json.notes.Length; i++) {
            int note = json.notes[i];
            double beat = json.beats[i];
            Vector3 position = new Vector3((note - 1) * note_width, 0, ((float) beat) * note_spacing);
            createNote(position);
        }
    }
}
