using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemUnitUpgrade : UIBase
{
    enum Buttons
    {
        UnitUpgradeBtn,
    }

    enum Texts
    {
        UnitLevel,
    }

    enum Images
    {
       UnitIcon,
    }

    int _unitId;
    UIScenePrepare _parentUI;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        BindEvent(GetButton((int)Buttons.UnitUpgradeBtn).gameObject, (PointerEventData data) => UpgradeUnit(data));

        // 아이콘 표시
        GetImage((int)Images.UnitIcon).sprite = Managers.UI.GetSkillIcon(_unitId);

        // 유닛 레벨에 따라 버튼 문구를 변경
        UpdateElements();
    }

    public void SetUnitId(int unitId)
    {
        _unitId = unitId;
    }

    public void SetParent(UIScenePrepare ui)
    {
        _parentUI = ui;
    }

    public void UpgradeUnit(PointerEventData data)
    {
        Managers.Status.LevelUpUnit(_unitId);
        UpdateElements();

        // 스킬 포인트에 변화가 생겼을 경우 다른 UI요소들도 갱신함
        _parentUI.UpdateAllUIs();
    }

    // 각 요소들을 최신 상태로 갱신함
    public void UpdateElements()
    {
        UpdateLevelTxt();
        UpdateUpgradeBtn();
    }

    private void UpdateUpgradeBtn()
    {
        // 포인트가 없을 경우
        if (Managers.Status.Point <= 0)
        {
            Button btn = GetButton((int)Buttons.UnitUpgradeBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }

        if (Managers.Status.IsAvailableUnit(_unitId))
        {
            if (Managers.Status.IsMaxLevelUnit(_unitId))
            {
                Button btn = GetButton((int)Buttons.UnitUpgradeBtn);
                btn.interactable = false;
                RemoveEvent(btn.gameObject);
            }
        }
    }

    private void UpdateLevelTxt()
    {
        // 활성 유닛인 경우 
        if (Managers.Status.IsAvailableUnit(_unitId))
        {
            if (Managers.Status.IsMaxLevelUnit(_unitId))
            {
                GetText((int)Texts.UnitLevel).text = UIConf.SKILL_LEVEL_MAX_TXT;
            }
            else
            {
                GetText((int)Texts.UnitLevel).text = Managers.Status.GetUnitLevel(_unitId).ToString();
            }
        }
        else
        {
            GetText((int)Texts.UnitLevel).text = UIConf.SKILL_NO_LEVEL_TXT;
        }
    }
}
