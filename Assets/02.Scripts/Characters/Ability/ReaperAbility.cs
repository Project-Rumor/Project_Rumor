using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAbility : Ability
{
    public override void Active()
    {
        CC.moveSpeed += CC.chardata.skillvalue1;
        // 시야
        CC.attackRange += CC.chardata.skillvalue3;
    }
}