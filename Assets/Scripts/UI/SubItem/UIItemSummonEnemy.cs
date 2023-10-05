using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSummonEnemy : UIBase
{
    enum Buttons
    {
        UIItemSummonEnemy,
    }

    enum Texts
    {
        UIItemSummonEnemyTxt,
    }

    //유닛 이미지가 생길때까지 임시로 이름을 표시
    //======================차후 수정==================
    string _enemyName;


    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.UIItemSummonEnemyTxt).text = _enemyName;
        BindEvent(GetButton((int)Buttons.UIItemSummonEnemy).gameObject, (PointerEventData data) => SummonEnemy(data));
    }

    public void SetName(string name)
    {
        _enemyName = name;
    }

    public void SummonEnemy(PointerEventData data)
    {
        Managers.Game.Spawn(Define.Layer.Monster, $"Monsters/{_enemyName}");
    }
}
