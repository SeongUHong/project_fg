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
        UnitUpgradeTxt,
        UnitName,
        UnitLevel,
    }

    string _activeUnitUpgradeTxt = "Level Up";
    string _inactiveUnitUpgradeTxt = "Get";

    string _inactiveUnitLevelTxt = "None";

    // ���� �̹����� ���涧���� �ӽ÷� �̸��� ǥ��
    // ======================���� ����==================
    int _unitId;
    string _name;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.UnitName).text = _name;

        // ���� Ȱ�� ���ο� ���� ����, ���׷��̵� ��ư ������ ����
        string upgradeTxt = _inactiveUnitUpgradeTxt;
        string levelTxt = _inactiveUnitLevelTxt;
        if (Managers.Status.IsAvailableUnit(_unitId))
        {
            upgradeTxt = _activeUnitUpgradeTxt;

            int level = Managers.Status.GetUnitLevel(_unitId);
            if (level != 0)
            {
                levelTxt = level.ToString();
            }
        }
        GetText((int)Texts.UnitUpgradeTxt).text = upgradeTxt;
        GetText((int)Texts.UnitLevel).text = levelTxt;

        BindEvent(GetButton((int)Buttons.UnitUpgradeBtn).gameObject, (PointerEventData data) => UpgradeUnit(data));
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetUnitId(int unitId)
    {
        _unitId = unitId;
    }

    public void UpgradeUnit(PointerEventData data)
    {
        int level = Managers.Status.LevelUpUnit(_unitId);
        GetText((int)Texts.UnitLevel).text = level.ToString();
        GetText((int)Texts.UnitUpgradeTxt).text = _activeUnitUpgradeTxt;
    }
}
