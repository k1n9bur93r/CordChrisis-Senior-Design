using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class JsonTrack
{
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

    GameObject createNote(Vector3 position) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        return cube;
    }

    JsonTrack readJsonFile(string filename) {
        string json_string = Resources.Load<TextAsset>(filename).ToString();
        JsonTrack json = JsonUtility.FromJson<JsonTrack>(json_string);
        return json;
    }

    void Start()
    {
        JsonTrack json = readJsonFile(track_file);

        Debug.Log(json.notes);
        if (json.notes.Length != json.beats.Length) {
            throw new System.ArrayTypeMismatchException("Invalid Json file, notes and beats length don't match.");
        }

        for (int i = 0; i < json.notes.Length; i++) {
            int note = json.notes[i];
            double beat = json.beats[i];
            Vector3 position = new Vector3((note - 1) * note_width, 0, ((float) beat) * note_spacing);
            createNote(position);
        }
    }
}
