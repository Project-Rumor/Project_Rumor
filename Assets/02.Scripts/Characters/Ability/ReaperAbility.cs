using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAbility : Ability
{
    public void ReaperPassive()
    {
        CC.moveSpeed += CC.chardata.skillvalue1;
        CC.Sight.intensity += CC.chardata.skillvalue2;
        CC.attackRange += CC.chardata.skillvalue3;
    }
}