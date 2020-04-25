using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metadata : MonoBehaviour
{
    public static bool isPanelUp;
    public GameObject metadataPanel;

    void Start()
    {
        isPanelUp = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPanelUp)
            {
                HidePanel();
            }
            else
            {
                ShowPanel();
            }
        }
    }
    
    void ShowPanel()
    {
        metadataPanel.SetActive(true);
        Time.timeScale = 0;
        isPanelUp = true;
    }

    public void HidePanel()
    {
        metadataPanel.SetActive(false);
        Time.timeScale = 1;
        isPanelUp = false;
    }

}
