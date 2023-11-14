using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectStat : Stat
{

    protected override void OnDead()
    {

        if (gameObject.GetComponent<MonsterCrystalController>() != null)
        {
            gameObject.GetComponent<MonsterCrystalController>().Destroy();
        }

    }
}
