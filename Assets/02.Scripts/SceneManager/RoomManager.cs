using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text PlayerCntText;
    [SerializeField] GameObject StartBtnBlur;

    int readyPlayerCnt;

    // Start is called before the first frame update
    void Start()
    {
        readyPlayerCnt = 0;

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            //if(readyPlayerCnt == PhotonNetwork.CurrentRoom.PlayerCount)
            //{
            //    StartBtnBlur.SetActive(false);
            //}
            //else
            //{
            //    StartBtnBlur.SetActive(true);
            //}
        }

        if (PhotonNetwork.InRoom)
            PlayerCntText.text = readyPlayerCnt + " / " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }

    public void Button_ReadyOrStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyPlayerCnt == PhotonNetwork.CurrentRoom.PlayerCount)
            {

            }
        }
        else
        {
            
        }
    }

    public void Button_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Scene_01_Lobby");
    }
}
