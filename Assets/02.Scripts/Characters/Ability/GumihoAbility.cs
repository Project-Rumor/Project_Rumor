using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumihoAbility : Ability
{
    public override void Active()
    {
        SoundManager.instance.PlaySFX("Gumiho");
    }
}
