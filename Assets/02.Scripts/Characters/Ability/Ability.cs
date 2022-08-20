using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
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
