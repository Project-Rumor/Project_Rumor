using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreatePlayer : MonoBehaviour
{
    public CharacterCtrl Create()
    {
        return PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<CharacterCtrl>();
    }
}
