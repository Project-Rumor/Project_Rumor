using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreatePlayer : MonoBehaviour
{
    public void Start()
    {
        GameObject inst = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
    public void Setup(string _code)
    {
        GameObject inst = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        inst.GetComponent<CharacterCtrl>().Setup(_code);
    }
}
