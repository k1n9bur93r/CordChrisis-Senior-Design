using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentBeat : MonoBehaviour
{
    public EditorNoteController editorController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "current beat: " + editorController.curBeat;
    }
}
