using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InGameManager : MonoBehaviour
{
    [SerializeField] CreatePlayer createPlayer;
    [SerializeField] GameStartPanel gameStartPanel;

    string charCode = "";

    List<string> charCodeList = new List<string>();

    void Start()
    {
        createPlayer.Setup(charCode);
        gameStartPanel.Setup(charCode);
    }

    void MixCharCode()
    {
        List<string> tempList = new List<string>();
        foreach (var data in TitleData.instance.charDatas)
        {
            tempList.Add(data.Key);
        }

        int cnt = tempList.Count;
        for (int i = 0; i < cnt; i++)
        {
            int rand = Random.Range(0, charCodeList.Count);
            charCodeList.Add(tempList[rand]);
            tempList.RemoveAt(rand);
        }
    }

    [PunRPC]
    public void SendCharCode()
    {

    }
}
