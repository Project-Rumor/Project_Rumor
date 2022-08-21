using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class ReaperAbility : Ability
{
    public void ReaperPassive()
    {
        CC.PV.RPC("IncreaseStat", RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    public void IncreaseStat()
    {
        CC.moveSpeed += CC.chardata.skillvalue1;
        CC.Sight.intensity += CC.chardata.skillvalue2;
        CC.attackRange += CC.chardata.skillvalue3;
    }
}