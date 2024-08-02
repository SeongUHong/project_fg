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
    }

    enum Objects
    {
        UnitSummonPanel,
    }

    //�̹����� ����� ���� ����
    string[] _unitItems = new string[5];

    public override void Init()
    {
        //���̽�ƽ�� �ڵ鷯 �߰�
        Bind<Image>(typeof(Images));
        BindJoyStickEvent(GetImage((int)Images.JoyStick).gameObject, GetImage((int)Images.Handle).gameObject);
        
        //�⺻���� ��ư
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.AttackBtn).gameObject, (PointerEventData data) => AttackEvent(data));

        //ĳ���� ��ȯâ
        Bind<GameObject>(typeof(Objects));
        GameObject unitSummonPanel = Get<GameObject>((int)Objects.UnitSummonPanel);

        foreach (CharacterConf.Unit unit in Enum.GetValues(typeof(CharacterConf.Unit)))
        {
            UIItemSummonUnit item = Managers.UI.MakeSubItem<UIItemSummonUnit>(unitSummonPanel.transform);
            item.SetName(unit.ToString());
        }

    }

}
