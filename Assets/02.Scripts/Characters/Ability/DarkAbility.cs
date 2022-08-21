using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class DarkAbility : Ability
{
    public override void Active()
    {
        Debug.Log("Dark!");
        SoundManager.instance.PlaySFX("Dark");
        CC.PV.RPC("DecreseSight", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void DecreseSight()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach(GameObject player in players)
        {
            CharacterCtrl TempCC = player.GetComponent<CharacterCtrl>();

            if (TempCC == CC)
                continue;
            else
            {
                CC.StartCoroutine(CC.SightChange(CC.chardata.skillvalue1, CC.chardata.skillvalue2));
            }
        }
    }
}
