using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using DG.Tweening;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform RoomNameTrans;
    [SerializeField] Transform PlayerCntTrans;
    [SerializeField] Button ReadyOrStartButton;
    [SerializeField] PhotonView PV;

    int readyPlayerCnt;
    bool isready;
    ExitGames.Client.Photon.Hashtable playerCustomProperities = new ExitGames.Client.Photon.Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM("Room");

        GameObject go = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        go.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName;

        readyPlayerCnt = 1;
        isready = false;

        RoomNameTrans.GetComponentInChildren<Text>().text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyOrStartButton.GetComponentInChildren<Text>().text = "게임 시작";
        }
        else
        {
            ReadyOrStartButton.GetComponentInChildren<Text>().text = "게임 준비";
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.SetPlayerCustomProperties(playerCustomProperities);

        StartEff();
    }

    void StartEff()
    {
        RoomNameTrans.GetComponent<RectTransform>().DOAnchorPosY(80f, 1f).From().SetEase(Ease.OutBack);
        PlayerCntTrans.GetComponent<RectTransform>().DOAnchorPosY(1000, 2f).From().SetEase(Ease.OutBack);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyPlayerCnt == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                ReadyOrStartButton.interactable = true;
                ReadyOrStartButton.GetComponentInChildren<Animator>().SetBool("isActive", true);
            }
            else
            {
                ReadyOrStartButton.interactable = false;
                ReadyOrStartButton.GetComponentInChildren<Animator>().SetBool("isActive", false);
            }
        }

        if (PhotonNetwork.InRoom)
            PlayerCntTrans.GetComponentInChildren<Text>().text = readyPlayerCnt + " / " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        int cnt = 0;

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var photonPlayer in PhotonNetwork.PlayerList)
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
                //Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
                //hash["isPlay"] = true;
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
        if (readyStatus)
            readyPlayerCnt++;
        else
            readyPlayerCnt--;
    }

    public void Button_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Scene_01_Lobby");
    }

    public void Btn_mouseOver()
    {
        SoundManager.instance.PlaySFX("BtnOverlap");
    }

    public void Btn_Clicked()
    {
        SoundManager.instance.PlaySFX("BtnClick");
    }
}
