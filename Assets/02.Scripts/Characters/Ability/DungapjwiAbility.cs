using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungapjwiAbility : Ability
{
    public override void Active()
    {
        SoundManager.instance.PlaySFX("Rat");
    }
}
