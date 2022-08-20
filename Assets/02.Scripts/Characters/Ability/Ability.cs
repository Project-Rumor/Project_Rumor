using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Ability : MonoBehaviourPunCallbacks
{
    string abilityName = "";
    string abilityDescription = "";
    float abilityCoolTime = 0.0f;

    public CharacterCtrl CC;

    public void Start()
    {
        CC = GetComponent<CharacterCtrl>();
        abilityName = CC.chardata.abilityName;
        abilityDescription = CC.chardata.ablityDescription;
        abilityCoolTime = CC.chardata.ability;
    }
    public virtual void Active()
    {

    }
}
