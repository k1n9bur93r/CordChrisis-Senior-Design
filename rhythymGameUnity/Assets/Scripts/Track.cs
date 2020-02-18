using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class JsonTrack
{
    public int[] notes;
}

public class Track : MonoBehaviour
{
    public int note_spacing = 5;
    public int note_width = 5;

    private int[] int_notes;
    private GameObject[] notes;


    GameObject createNote(Vector3 position) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        return cube;
    }

    int[] convertToPrimitive(List<int> list) {
        int_notes = new int[list.Count];
        int i = 0;
        foreach (int entry in list)
        {
            int_notes[i] = entry;
            i++;
        }
        return int_notes;
    }

    // Start is called before the first frame update
    void Start()
    {
        // int_notes = new JavaScriptSerializer().Deserialize<List<int>>("[1, 1, 4, 1, 4, 3, 2, 1]");
        Debug.Log("test");
        JsonTrack json = JsonUtility.FromJson<JsonTrack>("{\"notes\": [1, 1, 4, 1, 4, 3, 2, 1]}");
        int_notes = json.notes;
        // Debug.Log("test");
        // foreach (int entry in json) {
        //     Debug.Log(entry);
        // }
        // int_notes = convertToPrimitive(json);

        notes = new GameObject[int_notes.Length];
        for (int i = 0; i < int_notes.Length; i++) {
            Vector3 position = new Vector3((int_notes[i] - 1) * note_width, 0, i * note_spacing);
            notes[i] = createNote(position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
