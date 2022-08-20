using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Threading;

public class InGameManager : MonoBehaviour
{
    [SerializeField] CreatePlayer createPlayer;
    [SerializeField] GameStartPanel gameStartPanel;
    [SerializeField] PhotonView PV;

    CharacterCtrl MyChar;

    int waitPlayerCnt;

    List<string> charCodeList = new List<string>();
    List<CharacterCtrl> AllPlayers = new List<CharacterCtrl>();

    public string[] SpiritName = { "Char_Gumiho", "Char_Doggebi", "Char_Reaper", "Char_Dark", "Char_Dungapjwi", "Char_Emugi" };
    List<string> Cycle = new List<string>();
    List<string> CycleTmp = new List<string>();

    void Start()
    {
        waitPlayerCnt = 0;
        MyChar = createPlayer.Create();

        PV.RPC("CharCreated", RpcTarget.MasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Initialize());
        }

        gameStartPanel.Setup(MyChar.chardata.code);

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
        }
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
    public void CharCreated()
    {
        if (PhotonNetwork.IsMasterClient)
            waitPlayerCnt++;
    }

    [PunRPC]
    public void InitializeChar(int viewID, string _code)
    {
        PhotonView.Find(viewID).GetComponent<CharacterCtrl>().Setup(_code);

        int idx = Cycle.FindIndex(x => x == _code);

        PhotonView.Find(viewID).GetComponent<CharacterCtrl>().target = idx == PhotonNetwork.CurrentRoom.PlayerCount - 1 ? Cycle[0] : Cycle[idx + 1];
    }
}
