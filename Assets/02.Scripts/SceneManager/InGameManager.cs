using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.LookDev;

public class InGameManager : MonoBehaviour
{
    [SerializeField] CreatePlayer createPlayer;
    [SerializeField] GameStartPanel gameStartPanel;
    [SerializeField] GameObject gameEndPanel;
    [SerializeField] SkillPanel skillPanel;

    [SerializeField] PhotonView PV;
    [SerializeField] GameObject LightPrefab;

    CharacterCtrl MyChar;

    List<string> charCodeList = new List<string>();
    List<CharacterCtrl> AllPlayers = new List<CharacterCtrl>();

    public string[] SpiritName = { "Char_Gumiho", "Char_Doggebi", "Char_Reaper", "Char_Dark", "Char_Dungapjwi", "Char_Emugi" };
    List<string> Cycle = new List<string>();
    List<string> CycleTmp = new List<string>();

    void Start()
    {
        MyChar = createPlayer.Create();

        MyChar.Sight = Instantiate(LightPrefab, MyChar.gameObject.transform).GetComponent<Light2D>();

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Initialize());
        }

        //printInfoToError();
    }

    void Update()
    {
        
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
        }

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
    public void InitializeCycle(string _cycle)
    {
        Cycle.Add(_cycle);
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
        // game end panel
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
}
