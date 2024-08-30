using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScenePrepare : UIScene
{
    enum Objects
    {
        UnitListPanel,
        SkillListPanel,
    }

    enum Texts
    {
        StageId,
        UpgradePoint,
        PlayerLevel,
    }

    enum Buttons
    {
        NextStageBtn,
        PlayerLevelUpBtn,
    }

    List<UIItemUnitUpgrade> _upgradeItemUIs = new List<UIItemUnitUpgrade>();
    List<UIItemSkillUpgrade> _upgradeSkillUIs = new List<UIItemSkillUpgrade>();

    public override void Init()
    {
        Bind<GameObject>(typeof(Objects));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // ��������ID ���
        Text stageIdTxt = Get<Text>((int)Texts.StageId);
        stageIdTxt.text = Managers.Status.StageId.ToString();

        GameObject unitListPanel = Get<GameObject>((int)Objects.UnitListPanel);

        foreach (CharacterConf.Unit unit in Enum.GetValues(typeof(CharacterConf.Unit)))
        {
            UIItemUnitUpgrade item = Managers.UI.MakeSubItem<UIItemUnitUpgrade>(unitListPanel.transform);
            // ����UI�� �̰����� ���� 
            _upgradeItemUIs.Add(item);

            item.SetName(unit.ToString());
            item.SetUnitId((int)unit);
            item.SetParent(this);
        }

        // ��ų ���׷��̵� UI ����
        GameObject skillListPanel = Get<GameObject>((int)Objects.SkillListPanel);

        foreach (SkillConf.PlayerSkill skill in Enum.GetValues(typeof(SkillConf.PlayerSkill)))
        {
            UIItemSkillUpgrade item = Managers.UI.MakeSubItem<UIItemSkillUpgrade>(skillListPanel.transform);
            // ����UI�� �̰����� ���� 
            _upgradeSkillUIs.Add(item);

            item.SetName(skill.ToString());
            item.SetSkillId((int)skill);
            item.SetParent(this);
        }

        BindEvent(GetButton((int)Buttons.NextStageBtn).gameObject, LoadGameScene);
        BindEvent(GetButton((int)Buttons.PlayerLevelUpBtn).gameObject, LevelUpPlayer);

        // ��ų ����Ʈ
        UpdateAllUIs();
    }

    //�� �̵�
    public void LoadGameScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.GameScene);
    }

    // �÷��̾� ���� ��
    public void LevelUpPlayer(PointerEventData data)
    {
        Managers.Status.LevelUpPlayer();
        UpdateAllUIs();
    }

    // ��� UI�� �ֽ�ȭ
    public void UpdateAllUIs()
    {
        UpdatePoint();
        UpdateUIs();
        UpdateSubItemUIs();
    }

    // ��ų ����Ʈ ǥ�� ����
    public void UpdatePoint()
    {
        Get<Text>((int)Texts.UpgradePoint).text = Managers.Status.Point.ToString();
    }

    // UI ǥ�ø� ������
    public void UpdateUIs()
    {
        UpdateBtnTxts();
        UpdateSkillLevelTxts();
    }

    // UI�� ������ ������
    public void UpdateBtnTxts()
    {
        // ����Ʈ�� ���� ���
        if (Managers.Status.Point <= 0)
        {
            Button btn = GetButton((int)Buttons.PlayerLevelUpBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }

        // �÷��̾� ����
        if (Managers.Status.IsMaxPlayerLevel())
        {
            Button btn = GetButton((int)Buttons.PlayerLevelUpBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }
    }

    // ���� ǥ�ø� ������
    public void UpdateSkillLevelTxts()
    {
        // �÷��̾� ����
        if (Managers.Status.IsMaxPlayerLevel())
        {
            GetText((int)Texts.PlayerLevel).text = UIConf.SKILL_LEVEL_MAX_TXT;
        }
        else
        {
            GetText((int)Texts.PlayerLevel).text = Managers.Status.PlayerLevel.ToString();
        }
    }

    // �� UI�����۵��� ������
    public void UpdateSubItemUIs()
    {
        if (Managers.Status.Point > 0)
            return;

        foreach (UIItemUnitUpgrade ui in _upgradeItemUIs)
        {
            ui.UpdateElements();
        }
    }
}
