using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Room")]
    [SerializeField] Button roomPrefab;
    [SerializeField] Transform roomContent;
    List<RoomInfo> myRoomList = new List<RoomInfo>();

    [Header("Button")]
    [SerializeField] Button createRoomButton;
    [SerializeField] InputField createRoomNameInput;
    [SerializeField] Button joinRoomRandomButton;
    [SerializeField] Button disconnectButton;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        myRoomList.Clear();

        createRoomButton.onClick.AddListener(() =>
        {
            CreateRoomBtnEvent();
        });

        joinRoomRandomButton.onClick.AddListener(() =>
        {
            JoinRandomRoomBtnEvent();
        });

        disconnectButton.onClick.AddListener(() =>
        {
            PhotonNetwork.Disconnect();
        });

        for (int i = 0; i < 10; i++)
        {
            GameObject Inst = Instantiate(roomPrefab.gameObject, roomContent);
            Inst.SetActive(false);
        }
    }

    void CreateRoomBtnEvent()
    {
        if (createRoomNameInput.text == "")
        {
            Debug.LogWarning("Null RoomName");
            return;
        }
        if (createRoomNameInput.text.Length >= 10)
        {
            Debug.LogWarning("Long RoomName");
            return;
        }

        PhotonNetwork.CreateRoom(createRoomNameInput.text, new RoomOptions { MaxPlayers = 6 });
    }

    void JoinRandomRoomBtnEvent()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void JoinSelectRoom(int _idx)
    {
        PhotonNetwork.JoinRoom(myRoomList[_idx].Name);
    }

    void MyRoomListRenewal()
    {
        if (roomContent.childCount < myRoomList.Count)
        {
            for (int i = 0; i < myRoomList.Count; i++)
            {
                if (i >= roomContent.childCount)
                {
                    GameObject inst = Instantiate(roomPrefab.gameObject, roomContent);
                }

                SetupRoomInfo(i);
            }

        }
        else if (roomContent.childCount > myRoomList.Count)
        {
            for (int i = 0; i < roomContent.childCount; i++)
            {
                if (i >= myRoomList.Count)
                {
                    roomContent.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    SetupRoomInfo(i);
                }
            }
        }
    }

    void SetupRoomInfo(int _idx)
    {
        int index = _idx;

        Transform trans = roomContent.GetChild(index);
        Button btn = trans.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            JoinSelectRoom(index);
        });

        trans.GetChild(0).GetComponent<Text>().text = myRoomList[index].Name;
        trans.GetChild(1).GetComponent<Text>().text = myRoomList[index].PlayerCount + "/" + myRoomList[index].MaxPlayers;

        trans.gameObject.SetActive(true);
    }

    //Photon Override
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        NetworkManager.instance.MoveScene(0);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCnt = roomList.Count;
        for (int i = 0; i < roomCnt; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myRoomList.Contains(roomList[i]))
                {
                    myRoomList.Add(roomList[i]);
                }
                else
                {
                    myRoomList[myRoomList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else if (myRoomList.IndexOf(roomList[i]) != -1)
            {
                myRoomList.RemoveAt(myRoomList.IndexOf(roomList[i]));
            }
        }

        MyRoomListRenewal();
    }

    public override void OnJoinedRoom()
    {
        NetworkManager.instance.MoveScene(2);
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
    //
}
