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

        BindEvent(GetButton((int)Buttons.UnitUpgradeBtn).gameObject, (PointerEventData data) => UpgradeUnit(data));

        // ���� ������ ���� ��ư ������ ����
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

        // ��ų ����Ʈ�� ��ȭ�� ������ ��� �ٸ� UI��ҵ鵵 ������
        _parentUI.UpdateAllUIs();
    }

    // �� ��ҵ��� �ֽ� ���·� ������
    public void UpdateElements()
    {
        UpdateLevelTxt();
        UpdateUpgradeBtn();
    }

    private void UpdateUpgradeBtn()
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
        // Ȱ�� ������ ��� 
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
