using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSummonUnit : UIBase
{
    enum Buttons
    {
        UIItemSummonUnit,
    }

    enum Images
    {
        UnitIcon,
    }

    int _unitId;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        BindEvent(GetButton((int)Buttons.UIItemSummonUnit).gameObject, (PointerEventData data) => SummonUnit(data));

        // ���� ������ ǥ��
        // UISceneGame���� ������ ���ÿ� SetUnitId()�� ��������� ȣ��
        // Start()�� ��ü ���� ���� �����ӿ��� ȣ��Ǳ� ������ SetUnitId()�� ���� �����
        Sprite icon = Managers.UI.GetUnitIcon(_unitId);
        GetImage((int)Images.UnitIcon).sprite = icon;
    }

    public void SetUnitId(int unitId)
    {
        _unitId = unitId;
    }

    // ��ȯ
    public void SummonUnit(PointerEventData data)
    {
        Managers.Game.SummonUnit(_unitId);
    }
}
