using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCrystalController : StaticObjectController
{
    //스텟
    Stat _stat;

    //전단계 클리어
    int allClear;

    //넥스트 스테이지 판넬
    Panel_NextStage panel_NextStage;
    //올클리어 판넬
    All_Clear_Panel panel_AllClear;

    public override void Init()
    {

        _stat = gameObject.GetComponent<Stat>();
        if (_stat == null)
        {
            Debug.Log("Can't Load Stat Component");
        }
        _stat.SetStat(Managers.Data.GetStatByLevel("MonsterCrystalStat", 1));


        //HP바 추가
        if (gameObject.GetComponentInChildren<UIHpBar>() == null)
        {
            Managers.UI.MakeWorldUI<UIHpBar>(transform);
        }

        panel_NextStage = Managers.Game.NextPanel;
        panel_AllClear = Managers.Game.ClearPanel;
    }

    public override void Destroy()
    {
        Conf.Main.CLEAR_FLAG = true;
        transform.gameObject.SetActive(false);
        allClear = Conf.Main.CURRENT_STAGE;
        switch (allClear)
        {
            case 4:
                panel_AllClear.Show();
                break;

            default:
                panel_NextStage.Show();
                break;

        }
        //Managers.Resource.Destroy(gameObject);
    }
}
