using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Variables & setup

    [SerializeField] Text connectStatus;
    [SerializeField] Text RoomStatus;

    [SerializeField] InputField InputField_NickName;
    [SerializeField] InputField InputField_RoomName;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.AutomaticallySyncScene = true;

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    private void Update()
    {
        connectStatus.text = PhotonNetwork.NetworkClientState.ToString();

        if(PhotonNetwork.InRoom)
        {
            RoomStatus.text = PhotonNetwork.CurrentRoom.Name;
            RoomStatus.text += " / Player Cnt : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }


    #region PhotonCallbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = InputField_NickName.text;
        Debug.Log("Connected To Server");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Scene_02_Room");
        Debug.Log("Joined Room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Create Room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Join Room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Join Room Random");
    }
    #endregion


    #region Buttons
    public void Button_Connect()
    {
        PhotonNetwork.ConnectUsingSettings();

        Button_JoinLobby(); // 일단 단일 로비로 할 것 같으니까 connect 할 때 바로 로비로 접속
    }

    public void Button_Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void Button_JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void Button_CreateRoom()
    {
        string RoomName = "";

        for(int i = 0; i < 6; i++)
        {
            char c = (char)('A' + Random.Range(0, 26));
            RoomName += c;
        }

        PhotonNetwork.CreateRoom(RoomName, new RoomOptions { MaxPlayers = 6 });
    }

    public void Button_JoinRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(InputField_RoomName.text, new RoomOptions { MaxPlayers = 6 }, null);
    }

    public void Button_JoinRoomRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Button_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Button_GameStart()
    {
        if(PhotonNetwork.IsMasterClient)
            SceneManager.LoadScene("Scene_03_Game");
    }

    #endregion

    #region RPC

    #endregion


    [ContextMenu("Info")]
    void Info()
    {
        print("Player NickName : " + PhotonNetwork.LocalPlayer.NickName);

        if(PhotonNetwork.InRoom)
        {
            print("isMasterClient ? : " + PhotonNetwork.IsMasterClient);
            print("Room Name : " + PhotonNetwork.CurrentRoom.Name);
            print("Room now Player Cnt : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("Room Max Player Cnt : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playersName = "Player List : ";

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                playersName += PhotonNetwork.PlayerList[i].NickName + ", ";

            print(playersName);
        }
        else
        {
            print("Connected Player Cnt : " + PhotonNetwork.CountOfPlayers);
            print("Room Cnt : " + PhotonNetwork.CountOfRooms);
            print("Player Cnt In Room : " + PhotonNetwork.CountOfPlayersInRooms);
            print("isConnected Lobby ? : " + PhotonNetwork.InLobby);
            print("isConnected Server ? : " + PhotonNetwork.IsConnected);
        }
    }
}
