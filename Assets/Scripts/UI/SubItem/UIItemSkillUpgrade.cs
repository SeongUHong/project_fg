using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSkillUpgrade : UIBase
{
    enum Buttons
    {
        SkillLevelUpBtn,
    }

    enum Texts
    {
        SkillLevelUpTxt,
        SkillName,
        SkillLevel,
    }

    int _skillId;
    string _name;
    UIScenePrepare _parentUI;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.SkillName).text = _name;

        BindEvent(GetButton((int)Buttons.SkillLevelUpBtn).gameObject, (PointerEventData data) => UpgradeSkill(data));

        // スキルのレベルに応じて文言変更
        UpdateElements();
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetSkillId(int skillId)
    {
        _skillId = skillId;
    }

    public void SetParent(UIScenePrepare ui)
    {
        _parentUI = ui;
    }

    public void UpgradeSkill(PointerEventData data)
    {
        Managers.Status.LevelUpSkill(_skillId);
        UpdateElements();

        // スキルポイントに変化があったので、全てのUIを更新する
        _parentUI.UpdateAllUIs();
    }

    // 各要素を最新化する
    public void UpdateElements()
    {
        UpdateLevelTxt();
        UpdateLevelUpBtn();
    }

    private void UpdateLevelUpBtn()
    {
        // ポイントがない場合
        if (Managers.Status.Point <= 0)
        {
            Button btn = GetButton((int)Buttons.SkillLevelUpBtn);
            btn.interactable = false;
            RemoveEvent(btn.gameObject);
        }

        if (Managers.Status.IsAvailableSkill(_skillId))
        {
            GetText((int)Texts.SkillLevelUpTxt).text = UIConf.SKILL_LEVEL_UP_TXT;
            if (Managers.Status.IsMaxLevelSkill(_skillId))
            {
                Button btn = GetButton((int)Buttons.SkillLevelUpBtn);
                btn.interactable = false;
                RemoveEvent(btn.gameObject);
            }
        }
        else
        {
            GetText((int)Texts.SkillLevelUpTxt).text = UIConf.SKILL_GET_TXT;
        }
    }

    private void UpdateLevelTxt()
    {
        // 活性されたスキルではない場合
        if (Managers.Status.IsAvailableSkill(_skillId))
        {
            if (Managers.Status.IsMaxLevelSkill(_skillId))
            {
                GetText((int)Texts.SkillLevel).text = UIConf.SKILL_LEVEL_MAX_TXT;
            }
            else
            {
                GetText((int)Texts.SkillLevel).text = Managers.Status.GetSkillLevel(_skillId).ToString();
            }
        }
        else
        {
            GetText((int)Texts.SkillLevel).text = UIConf.SKILL_NO_LEVEL_TXT;
        }
    }
}
