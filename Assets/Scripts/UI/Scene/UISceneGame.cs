using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISceneGame : UIScene
{
    enum Images
    {
        JoyStick,
        Handle,
    }

    enum Buttons
    {
        AttackBtn,
    }

    enum Object
    {
        UnitSummonPanel,
        EnemySummonPanel,
        Panel_GameOver,
    }

    enum Object_Enemy
    {
        EnemySummonPanel,
    }

    //이미지가 생기면 차후 수정
    string[] _enemyItems = new string[5];
    string[] _unitItems = new string[5];
    

    public override void Init()
    {
        //조이스틱에 핸들러 추가
        Bind<Image>(typeof(Images));
        BindJoyStickEvent(GetImage((int)Images.JoyStick).gameObject, GetImage((int)Images.Handle).gameObject);
        
        //기본공격 버튼
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.AttackBtn).gameObject, (PointerEventData data) => AttackEvent(data));

        //캐릭터 소환창
        Bind<GameObject>(typeof(Object));
        GameObject unitSummonPanel = Get<GameObject>((int)Object.UnitSummonPanel);

        //적 소환창
        //Bind<GameObject>(typeof(Object_Enemy));
        GameObject enemySummonPanel = Get<GameObject>((int)Object.EnemySummonPanel);

        //재도전 팝업
        //GameObject Panel_GameOver = Get<GameObject>((int)Object.Panel_GameOver);


        //소환창에 캐릭터 버튼을 추가
        //==============수정 필요====================
        //차후 보유 캐릭터를 불러와서 버튼 생성하도록 수정
        _unitItems[0] = "FitnessGirlSniper";
        _unitItems[1] = "OfficeGirlKnight";
        _enemyItems[0] = "Green";
        _enemyItems[1] = "Blue";
        _enemyItems[2] = "Red";
        _enemyItems[3] = "Purple";

        foreach (string unitItem in _unitItems)
        {
            UIItemSummonUnit uiItem = Managers.UI.MakeSubItem<UIItemSummonUnit>(unitSummonPanel.transform);
            if (!string.IsNullOrEmpty(unitItem))
            {
                uiItem.SetName(unitItem);
            }
        }

        foreach (string enemyItem in _enemyItems)
        {
            UIItemSummonEnemy uiEnemyItem = Managers.UI.MakeSubItem<UIItemSummonEnemy>(enemySummonPanel.transform);
            if (!string.IsNullOrEmpty(enemyItem))
            {
                uiEnemyItem.SetName(enemyItem);
            }
        }
        //Panel_GameOver panel_GameOver = Managers.UI.MakePopUp<Panel_GameOver>(Panel_GameOver.transform);
    }

}
