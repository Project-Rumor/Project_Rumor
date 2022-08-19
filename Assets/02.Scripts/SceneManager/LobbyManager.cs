using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Room")]
    [SerializeField] Button roomPrefab;
    [SerializeField] Transform roomContent;
    List<RoomInfo> myRoomList = new List<RoomInfo>();

    [Header("Button")]
    [SerializeField] Button createRoomButton;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        myRoomList.Clear();

        createRoomButton.onClick.AddListener(() =>
        {
            PhotonNetwork.CreateRoom("123", new RoomOptions { MaxPlayers = 6 });
        });

        for (int i = 0; i < 10; i++)
        {
            GameObject Inst = Instantiate(roomPrefab.gameObject, roomContent);
            Inst.SetActive(false);
        }
    }

    void Update()
    {
        
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

                roomContent.GetChild(i).gameObject.SetActive(true);
            }

        }
        else if (roomContent.childCount > myRoomList.Count)
        {
            for (int i = 0; i < roomContent.childCount; i++)
            {
                if (i >= myRoomList.Count)
                {

                }
            }
        }
    }

    //Photon Override
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
        SceneManager.LoadScene("Scene_02_Room");
    }
    //
}
