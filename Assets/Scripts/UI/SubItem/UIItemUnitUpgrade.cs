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
        UnitName,
        UnitLevel,
    }

    // 유닛 이미지가 생길때까지 임시로 이름을 표시
    // ======================차후 수정==================
    int _unitId;
    string _name;
    UIScenePrepare _parentUI;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.UnitName).text = _name;

        BindEvent(GetButton((int)Buttons.UnitUpgradeBtn).gameObject, (PointerEventData data) => UpgradeUnit(data));

        // 유닛 레벨에 따라 버튼 문구를 변경
        UpdateElements();
    }

    public void SetName(string name)
    {
        _name = name;
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
