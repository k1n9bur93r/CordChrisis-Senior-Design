using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class JsonTrack
{
    // Disable warnings of the form:
    // 'JsonTrack.notes' is never assigned to, and will always have its default value null
    // It gets assigned to in the json serializer
    #pragma warning disable 0649
    public double[] notes;
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

        int c = 0;
        foreach (double entry in json.notes) {
            Vector3 position = new Vector3((((float) entry) - 1) * note_width, 0, c * note_spacing);
            createNote(position);
            c++;
        }
    }
}
