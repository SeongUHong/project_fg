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

        // 유닛 아이콘 표시
        // UISceneGame에서 생성과 동시에 SetUnitId()를 명시적으로 호출
        // Start()는 객체 생성 다음 프레임에서 호출되기 때문에 SetUnitId()가 먼저 실행됨
        Sprite icon = Managers.UI.GetUnitIcon(_unitId);
        GetImage((int)Images.UnitIcon).sprite = icon;
    }

    public void SetUnitId(int unitId)
    {
        _unitId = unitId;
    }

    // 소환
    public void SummonUnit(PointerEventData data)
    {
        Managers.Game.SummonUnit(_unitId);
    }
}
