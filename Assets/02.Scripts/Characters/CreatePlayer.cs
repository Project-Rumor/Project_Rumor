using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreatePlayer : MonoBehaviour
{
    public CharacterCtrl Create(int playerNumber)
    {
        return PhotonNetwork.Instantiate("Player" + playerNumber.ToString(), Vector3.zero, Quaternion.identity).GetComponent<CharacterCtrl>();
    }
}
