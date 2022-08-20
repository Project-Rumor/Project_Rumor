using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    void Awake()
    {
        instance = this;
    }

    string[] SceneName = {"Scene_00_Title",
                          "Scene_01_Lobby",
                          "Scene_02_Room",
                          "Scene_03_Game" };

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    public void MoveScene(int sceneNumber)
    {
        SceneManager.LoadScene(SceneName[sceneNumber]);
    }
}
