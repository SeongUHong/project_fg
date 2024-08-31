using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISceneGame : UIScene
{
    enum Images
    {
        JoyStick,
        Handle,
    }

    enum Buttons
    {
        AttackBtn,
        FlameBallBtn,
        RangeSkillBtn,
    }

    enum Objects
    {
        UnitSummonPanel,
        SummonGauge,
    }

    public override void Init()
    {
        //���̽�ƽ�� �ڵ鷯 �߰�
        Bind<Image>(typeof(Images));
        Managers.Input.BindJoyStickEvent(GetImage((int)Images.JoyStick).gameObject, GetImage((int)Images.Handle).gameObject);
        
        //�⺻���� ��ư
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.AttackBtn).gameObject, (PointerEventData data) => Managers.Input.ExecAttackEvent(data));

        // �� ��ư�� ��ų �̺�Ʈ�� ���ε�
        // FlameBall
        if (Managers.Status.IsAvailableSkill((int)SkillConf.Skill.FlameBall))
        {
            BindEvent(GetButton((int)Buttons.FlameBallBtn).gameObject, (PointerEventData data) => Managers.Input.ExecFlameBallEvent(data));
        }
        else
        {
            GetButton((int)Buttons.FlameBallBtn).interactable = false;
        }
        // FreezeCircle
        if (Managers.Status.IsAvailableSkill((int)SkillConf.Skill.FreezeCircle))
        {
            BindEvent(GetButton((int)Buttons.RangeSkillBtn).gameObject, (PointerEventData data) => Managers.Input.ExecFreezeCircleEvent(data));
        }
        else
        {
            GetButton((int)Buttons.RangeSkillBtn).interactable = false;
        }

        //ĳ���� ��ȯâ
        Bind<GameObject>(typeof(Objects));
        GameObject unitSummonPanel = Get<GameObject>((int)Objects.UnitSummonPanel);

        foreach (CharacterConf.Unit unit in Managers.Status.GetAvailableUnitIds())
        {
            UIItemSummonUnit item = Managers.UI.MakeSubItem<UIItemSummonUnit>(unitSummonPanel.transform);
            item.SetUnitId((int)unit);
        }
    }

    private void Update()
    {
        GetGameObject((int)Objects.SummonGauge).GetComponent<Slider>().value = Managers.Game.GetSummonGauge() / Define.MAX_SUMMON_GAUGE;
    }

}
