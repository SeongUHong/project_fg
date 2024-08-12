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
    string _unitMaxLevelTxt = "Max";

    // ���� �̹����� ���涧���� �ӽ÷� �̸��� ǥ��
    // ======================���� ����==================
    int _unitId;
    string _name;
    UIScenePrepare _parentUI;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.UnitName).text = _name;

        // ���� ������ ���� ��ư ������ ����
        UpdateElements();

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

    public void SetParent(UIScenePrepare ui)
    {
        _parentUI = ui;
    }

    public void UpgradeUnit(PointerEventData data)
    {
        Managers.Status.LevelUpUnit(_unitId);
        UpdateElements();

        // ��ų ����Ʈ�� ��ȭ�� ������ ��� �ٸ� UI��ҵ鵵 ������
        _parentUI.ExecUpgrade();
    }

    // �� ��ҵ��� �ֽ� ���·� ������
    public void UpdateElements()
    {
        UpdateLevelTxt();
        UpdateUpgradeTxt();
    }

    private void UpdateUpgradeTxt()
    {
        // ����Ʈ�� ���� ���
        if (Managers.Status.Point <= 0)
        {
            Button btn = GetButton((int)Buttons.UnitUpgradeBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }

        if (Managers.Status.IsAvailableUnit(_unitId))
        {
            GetText((int)Texts.UnitUpgradeTxt).text = _activeUnitUpgradeTxt;
            if (Managers.Status.IsMaxLevelUnit(_unitId))
            {
                Button btn = GetButton((int)Buttons.UnitUpgradeBtn);
                btn.interactable = false;
                RemoveEvent(btn.gameObject);
            }
        }
        else
        {
            GetText((int)Texts.UnitUpgradeTxt).text = _inactiveUnitUpgradeTxt;
        }
    }

    private void UpdateLevelTxt()
    {
        // Ȱ�� ������ ��� 
        if (Managers.Status.IsAvailableUnit(_unitId))
        {
            if (Managers.Status.IsMaxLevelUnit(_unitId))
            {
                GetText((int)Texts.UnitLevel).text = _unitMaxLevelTxt;
            }
            else
            {
                GetText((int)Texts.UnitLevel).text = Managers.Status.GetUnitLevel(_unitId).ToString();
            }
        }
        else
        {
            GetText((int)Texts.UnitLevel).text = _inactiveUnitLevelTxt;
        }
    }
}
