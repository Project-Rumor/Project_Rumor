using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InGameUIState
{
    None,
    Play,
    Exit,
    Guide,
    Map,
}

public class InGameUICtrl : MonoBehaviour
{
    public InGameUIState inGameUIState = InGameUIState.None;

    [SerializeField] GuidePanel guidePanel;
    [SerializeField] ExitPanel exitPanel;
    [SerializeField] MapPanel mapPanel;

    void Update()
    {
        switch (inGameUIState)
        {
            case InGameUIState.None:
                break;

            case InGameUIState.Play:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    exitPanel.OpenPanel();
                    inGameUIState = InGameUIState.Exit;
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    guidePanel.OpenPanel();
                    inGameUIState = InGameUIState.Guide;
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    mapPanel.OpenPanel();
                    inGameUIState = InGameUIState.Map;
                }
                break;

            case InGameUIState.Exit:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    exitPanel.ClosePanel();
                    inGameUIState = InGameUIState.Play;
                }
                break;

            case InGameUIState.Guide:
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    guidePanel.ClosePanel();
                    inGameUIState = InGameUIState.Play;
                }
                break;

            case InGameUIState.Map:
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    mapPanel.ClosePanel();
                    inGameUIState = InGameUIState.Play;
                }
                break;
        }
    }
}
