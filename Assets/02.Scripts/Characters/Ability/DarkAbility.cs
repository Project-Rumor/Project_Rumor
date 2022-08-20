using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class DarkAbility : Ability
{
    public override void Active()
    {
        CC.PV.RPC("DecreseSight", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void DecreseSight()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        
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
