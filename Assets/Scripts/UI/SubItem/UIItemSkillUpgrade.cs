using UnityEngine;
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
        SkillLevel,
    }

    enum Images
    {
        SkillIcon,
    }

    int _skillId;
    UIScenePrepare _parentUI;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        BindEvent(GetButton((int)Buttons.SkillLevelUpBtn).gameObject, (PointerEventData data) => UpgradeSkill(data));

        // アイコン表示
        Sprite icon = Managers.UI.GetSkillIcon(_skillId);
        Image img = GetImage((int)Images.SkillIcon);
        img.sprite = icon;

        // スキルのレベルに応じて文言変更
        UpdateElements();
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
            if (Managers.Status.IsMaxLevelSkill(_skillId))
            {
                Button btn = GetButton((int)Buttons.SkillLevelUpBtn);
                btn.interactable = false;
                RemoveEvent(btn.gameObject);
            }
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
