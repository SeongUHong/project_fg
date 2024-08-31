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
    }

    public void SetUnitId(int unitId)
    {
        _unitId = unitId;
    }

    // º“»Ø
    public void SummonUnit(PointerEventData data)
    {
        Managers.Game.SummonUnit(_unitId);
    }
}
