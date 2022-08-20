﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmugiAbility : Ability
{
    public override void Active()
    {
        Debug.Log("유령 작전 개시");
        StartCoroutine(GhostMode());
    }

    IEnumerator GhostMode()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        CC.StartCoroutine(CC.MoveSpeedChange(CC.chardata.skillvalue1, CC.chardata.skillvalue2));

        yield return new WaitForSeconds(CC.chardata.skillvalue1);

        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
