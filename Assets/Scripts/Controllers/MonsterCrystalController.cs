using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCrystalController : StaticObjectController
{
    //½ºÅÝ
    Stat _stat;


    //³Ø½ºÆ® ½ºÅ×ÀÌÁö ÆÇ³Ú
    Panel_NextStage panel_NextStage;

    public override void Init()
    {

        _stat = gameObject.GetComponent<Stat>();
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }
        _stat.SetStat(Managers.Data.GetStatByLevel("MonsterCrystalStat", 1));


        //HP¹Ù Ãß°¡
        if (gameObject.GetComponentInChildren<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }

        panel_NextStage = Managers.Game.NextPanel;
    }

    public override void Destroy()
    {
        ControllerConf._clearFlag = true;
        transform.gameObject.SetActive(false);
        panel_NextStage.Show();
        //Managers.Resource.Destroy(gameObject);
    }
}
