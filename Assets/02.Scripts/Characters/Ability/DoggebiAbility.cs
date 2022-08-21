using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DoggebiAbility : Ability
{
    bool isAbilityOn = false;

    public override void Active()
    {
        StartCoroutine(AbilityOn());
    }

    IEnumerator AbilityOn()
    {
        isAbilityOn = true;
        CC.SR.color = new Color(CC.SR.color.r, CC.SR.color.g, CC.SR.color.b, 0.5f);
        CC.PV.RPC("Stealth", RpcTarget.Others);
        yield return new WaitForSeconds(10.0f);

        isAbilityOn = false;
        CC.SR.color = new Color(CC.SR.color.r, CC.SR.color.g, CC.SR.color.b, 1.0f);
        CC.PV.RPC("StealthEnd", RpcTarget.Others);
    }

    [PunRPC]
    void Stealth()
    {
        CC.SR.color = new Color(CC.SR.color.r, CC.SR.color.g, CC.SR.color.b, 0.0f);
    }

    [PunRPC]
    void StealthEnd()
    {
        CC.SR.color = new Color(CC.SR.color.r, CC.SR.color.g, CC.SR.color.b, 1.0f);
    }
}
