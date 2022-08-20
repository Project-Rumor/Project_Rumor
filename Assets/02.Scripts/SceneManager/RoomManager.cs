using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.Diagnostics;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text PlayerCntText;
    [SerializeField] Text ReadyOrStartText;
    [SerializeField] PhotonView PV;

    int readyPlayerCnt;
    bool isready;
    ExitGames.Client.Photon.Hashtable playerCustomProperities = new ExitGames.Client.Photon.Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        readyPlayerCnt = 1;
        isready = false;

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyOrStartText.text = "Start";
        }
        else
        {
            ReadyOrStartText.text = "Ready";
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.SetPlayerCustomProperties(playerCustomProperities);
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (readyPlayerCnt == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                ReadyOrStartText.GetComponentInParent<Button>().interactable = true;
            }
            else
            {
                ReadyOrStartText.GetComponentInParent<Button>().interactable = false;
            }
        }

        if (PhotonNetwork.InRoom)
            PlayerCntText.text = readyPlayerCnt + " / " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        int cnt = 0;

        if(PhotonNetwork.IsMasterClient)
        {
            foreach(var photonPlayer in PhotonNetwork.PlayerList)
            {
                if ((bool)photonPlayer.CustomProperties["PlayerReady"])
                    cnt++;
            }
        }

        readyPlayerCnt = cnt;
    }

    public void Button_ReadyOrStart()
    {
        Debug.Log("Button Pushed");

        if (PhotonNetwork.IsMasterClient)
        {
            if (readyPlayerCnt == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                NetworkManager.instance.MoveScene(3);
            }
        }
        else
        {
            isready = !isready;

            PV.RPC("ReadyStatusChange", RpcTarget.AllBuffered, isready);
        }
    }

    [PunRPC]
    public void ReadyStatusChange(bool readyStatus)
    {
        Debug.Log("Recived RPC");

        if(PhotonNetwork.IsMasterClient)
        {
            if (readyStatus)
                readyPlayerCnt++;
            else
                readyPlayerCnt--;
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
