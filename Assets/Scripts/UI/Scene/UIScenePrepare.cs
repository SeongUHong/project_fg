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

        // 스테이지ID 출력
        Text stageIdTxt = Get<Text>((int)Texts.StageId);
        stageIdTxt.text = Managers.Status.StageId.ToString();

        GameObject unitListPanel = Get<GameObject>((int)Objects.UnitListPanel);

        foreach (CharacterConf.Unit unit in Enum.GetValues(typeof(CharacterConf.Unit)))
        {
            UIItemUnitUpgrade item = Managers.UI.MakeSubItem<UIItemUnitUpgrade>(unitListPanel.transform);
            // 서브UI를 이곳에서 관리 
            _upgradeItemUIs.Add(item);

            item.SetName(unit.ToString());
            item.SetUnitId((int)unit);
            item.SetParent(this);
        }

        // 스킬 업그레이드 UI 전개
        GameObject skillListPanel = Get<GameObject>((int)Objects.SkillListPanel);

        foreach (SkillConf.PlayerSkill skill in Enum.GetValues(typeof(SkillConf.PlayerSkill)))
        {
            UIItemSkillUpgrade item = Managers.UI.MakeSubItem<UIItemSkillUpgrade>(skillListPanel.transform);
            // 서브UI를 이곳에서 관리 
            _upgradeSkillUIs.Add(item);

            item.SetName(skill.ToString());
            item.SetSkillId((int)skill);
            item.SetParent(this);
        }

        BindEvent(GetButton((int)Buttons.NextStageBtn).gameObject, LoadGameScene);
        BindEvent(GetButton((int)Buttons.PlayerLevelUpBtn).gameObject, LevelUpPlayer);

        // 스킬 포인트
        UpdateAllUIs();
    }

    //씬 이동
    public void LoadGameScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.GameScene);
    }

    // 플레이어 레벨 업
    public void LevelUpPlayer(PointerEventData data)
    {
        Managers.Status.LevelUpPlayer();
        UpdateAllUIs();
    }

    // 모든 UI를 최신화
    public void UpdateAllUIs()
    {
        UpdatePoint();
        UpdateUIs();
        UpdateSubItemUIs();
    }

    // 스킬 포인트 표시 갱신
    public void UpdatePoint()
    {
        Get<Text>((int)Texts.UpgradePoint).text = Managers.Status.Point.ToString();
    }

    // UI 표시를 갱신함
    public void UpdateUIs()
    {
        UpdateBtnTxts();
        UpdateSkillLevelTxts();
    }

    // UI의 문구를 갱신함
    public void UpdateBtnTxts()
    {
        // 포인트가 없을 경우
        if (Managers.Status.Point <= 0)
        {
            Button btn = GetButton((int)Buttons.PlayerLevelUpBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }

        // 플레이어 레벨
        if (Managers.Status.IsMaxPlayerLevel())
        {
            Button btn = GetButton((int)Buttons.PlayerLevelUpBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }
    }

    // 레벨 표시를 갱신함
    public void UpdateSkillLevelTxts()
    {
        // 플레이어 레벨
        if (Managers.Status.IsMaxPlayerLevel())
        {
            GetText((int)Texts.PlayerLevel).text = UIConf.SKILL_LEVEL_MAX_TXT;
        }
        else
        {
            GetText((int)Texts.PlayerLevel).text = Managers.Status.PlayerLevel.ToString();
        }
    }

    // 각 UI아이템들을 갱신함
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
