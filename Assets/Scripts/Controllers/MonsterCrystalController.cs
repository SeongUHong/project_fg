using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCrystalController : StaticObjectController
{
    //����
    Stat _stat;

    protected override void Init()
    {
        _stat = gameObject.GetComponent<Stat>();
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }
        _stat.SetStat(Managers.Data.GetStatByLevel("MonsterCrystalStat", 1));

        //HP�� �߰�
        if (gameObject.GetComponentInChildren<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }
    }

}
