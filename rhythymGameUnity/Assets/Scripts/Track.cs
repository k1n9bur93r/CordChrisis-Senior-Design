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
    public string track_file = "Text/basic";
    public float note_spacing = 5;
    public float note_width = 5;

    // public JsonTrack track

    // private double[] note_locations;
    // private GameObject[] notes;

    GameObject createNote(Vector3 position) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        return cube;
    }

    // double[] convertToPrimitive(List<double> list) {
    //     note_locations = new double[list.Count];
    //     int i = 0;
    //     foreach (int entry in list)
    //     {
    //         note_locations[i] = entry;
    //         i++;
    //     }
    //     return note_locations;
    // }

    JsonTrack readJsonFile() {
        string json_string = Resources.Load<TextAsset>("Text/basic").ToString();
        // Debug.Log(json_string);
        // Debug.Log(json_text_file);
        // Debug.Log(ta.ToString());
        JsonTrack json = JsonUtility.FromJson<JsonTrack>(json_string);
        return json;
    }

    // Start is called before the first frame update
    void Start()
    {
        // JsonTrack json = JsonUtility.FromJson<JsonTrack>("{\"notes\": [1, 1, 4, 1, 4, 3, 2, 1]}");
        // note_locations = json.notes;
        // Debug.Log("test");
        // foreach (int entry in json) {
        //     Debug.Log(entry);
        // }
        // note_locations = convertToPrimitive(json);

        JsonTrack json = readJsonFile();

        int c = 0;
        foreach (double entry in json.notes) {
            Vector3 position = new Vector3((((float) entry) - 1) * note_width, 0, c * note_spacing);
            createNote(position);
            c++;
        }

        // notes = new GameObject[note_locations.Length];
        // for (int i = 0; i < note_locations.Length; i++) {
        //     Vector3 position = new Vector3((note_locations[i] - 1) * note_width, 0, i * note_spacing);
        //     notes[i] = createNote(position);
        // }
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
