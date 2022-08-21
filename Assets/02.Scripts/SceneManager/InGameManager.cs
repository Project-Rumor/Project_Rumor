using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class InGameManager : Singleton<InGameManager>
{
    [SerializeField] CreatePlayer createPlayer;
    [SerializeField] GameStartPanel gameStartPanel;
    [SerializeField] GameObject gameEndPanel;
    [SerializeField] SkillPanel skillPanel;
    [SerializeField] CyclePanel cyclePanel;

    [SerializeField] PhotonView PV;
    [SerializeField] GameObject LightPrefab;

    [SerializeField] GameObject Notice;
    [SerializeField] GameObject CluePanel;
    [SerializeField] GameObject Clue;
    [SerializeField] List<Clue> AllClues;
    [SerializeField] List<string> ClueDescription = new List<string>();

    [SerializeField] Cinemachine.CinemachineVirtualCamera CV;

    CharacterCtrl MyChar;
    CharacterCtrl Winner;

    List<string> charCodeList = new List<string>();
    List<CharacterCtrl> AllPlayers = new List<CharacterCtrl>();

    public string[] SpiritName = { "Char_Gumiho", "Char_Doggebi", "Char_Reaper", "Char_Dark", "Char_Dungapjwi", "Char_Emugi" };
    public List<string> Cycle = new List<string>();
    List<string> CycleTmp = new List<string>();

    void Start()
    {
        SoundManager.instance.StopBGM();

        MyChar = createPlayer.Create(PhotonNetwork.LocalPlayer.ActorNumber);

        MyChar.Sight = Instantiate(LightPrefab, MyChar.gameObject.transform).transform.GetChild(0).GetComponent<Light2D>();

        CV.Follow = MyChar.gameObject.transform;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Initialize());
        }
        //printInfoToError();
    }

    IEnumerator Initialize()
    {
        while (AllPlayers.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            AllPlayers = FindObjectsOfType<CharacterCtrl>().ToList<CharacterCtrl>();

            yield return new WaitForSeconds(1f);
        }

        foreach (string s in SpiritName)
            CycleTmp.Add(s);

        CycleInitialize();
        List<string> selected = new List<string>();

        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            PV.RPC("InitializeCycle", RpcTarget.AllBuffered, CycleTmp[i]);
            selected.Add(CycleTmp[i]);
        }

        MixCharCode(selected);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            int PhotonViewID = AllPlayers[i].photonView.ViewID;

            PV.RPC("InitializeChar", RpcTarget.AllBuffered, PhotonViewID, charCodeList[i]);

            PV.RPC("InitializeName", RpcTarget.AllBuffered, PhotonViewID, i);

            string _clueInfo = PhotonNetwork.PlayerList[i].NickName + "의 혼은 " + TitleData.instance.charDatas[charCodeList[i]].name + "입니다.";

            PV.RPC("InitializeClueInfo", RpcTarget.AllBuffered, AllClues[i].gameObject.GetPhotonView().ViewID, _clueInfo);
        }

        PV.RPC("InitializeCyclePanel", RpcTarget.AllBuffered);

        PV.RPC("GameStart", RpcTarget.AllBuffered);
    }

    [ContextMenu("cycle Info")]
    void printInfoToError()
    {
        Debug.LogError("CyCle");

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            Debug.LogError(Cycle[i]);
        }

        Debug.Log("MyJob : " + MyChar.chardata.code);
        Debug.Log("Mytarget : " + MyChar.target);
    }

    void MixCharCode(List<string> _selected)
    {
        int cnt = _selected.Count;

        for (int i = 0; i < cnt; i++)
        {
            int rand = Random.Range(0, _selected.Count);
            charCodeList.Add(_selected[rand]);
            _selected.RemoveAt(rand);
        }
    }

    public void CycleInitialize()
    {
        Debug.Log("Initialize");

        var rnd = new System.Random();
        var randorder = CycleTmp.OrderBy(item => rnd.Next());

        List<string> spirtsTmp = new List<string>();

        foreach (var cycle in randorder)
        {
            spirtsTmp.Add(cycle);
        }


        CycleTmp = spirtsTmp;
    }

    [PunRPC]
    public void InitializeClueInfo(int viewID, string _clueInfo)
    {
        PhotonView.Find(viewID).GetComponent<Clue>().clueInfo = _clueInfo;
    }


    [PunRPC]
    public void InitializeCycle(string _cycle)
    {
        Cycle.Add(_cycle);
    }

    [PunRPC]
    public void InitializeCyclePanel()
    {
        cyclePanel.InitializeCycle();
    }

    [PunRPC]
    public void InitializeChar(int viewID, string _code)
    {
        PhotonView.Find(viewID).GetComponent<CharacterCtrl>().Setup(_code);

        int idx = Cycle.FindIndex(x => x == _code);

        PhotonView.Find(viewID).GetComponent<CharacterCtrl>().target = idx == PhotonNetwork.CurrentRoom.PlayerCount - 1 ? Cycle[0] : Cycle[idx + 1];
    }

    [PunRPC]
    public void InitializeName(int viewID, int playerIndex)
    {
        PhotonView.Find(viewID).gameObject.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[playerIndex].NickName;
    }

    [PunRPC]
    public void GameStart()
    {
        gameStartPanel.Setup(MyChar.chardata.code);
        skillPanel.Setup(MyChar.chardata.code);
    }


    [PunRPC]
    public void GameEnd()
    {
        gameEndPanel.SetActive(true);
        gameEndPanel.transform.GetChild(0).GetComponent<Text>().text = Winner.gameObject.transform.GetChild(1).GetChild(0)
                                                                        .GetComponent<TextMeshProUGUI>().text + " Win!!";

        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX("Win");
        // game end panel
    }

    public void SomeOneDied(CharacterCtrl DiedChar)
    {
        PV.RPC("CycleUpdate", RpcTarget.AllBuffered, DiedChar.chardata.code);

        AllPlayers.Remove(DiedChar);

        if(AllPlayers.Count == 1)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Winner = AllPlayers[0];
                GameEnd();
            }
        }
    }

    [PunRPC]
    public void CycleUpdate(string deadPlayerSpirit)
    {
        Cycle.Remove(deadPlayerSpirit);

        for(int i = 0; i < Cycle.Count; i++)
        {
            if (Cycle[i].Equals(MyChar.chardata.code))
            {
                if (i == Cycle.Count - 1)
                    MyChar.target = Cycle[0];
                else
                    MyChar.target = Cycle[i + 1];
            }
        }

    }

    public void Button_toRoom()
    {
        PhotonNetwork.LoadLevel(2);
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
