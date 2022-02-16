using System.Collections;
using System.Collections.Generic;
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

        //��ȯâ�� ĳ���� ��ư�� �߰�
        //==============���� �ʿ�====================
        //���� ���� ĳ���͸� �ҷ��ͼ� ��ư �����ϵ��� ����
        _unitItems[0] = "FitnessGirlSniper";
        _unitItems[1] = "OfficeGirlKnight";
        foreach(string unitItem in _unitItems)
        {
            UIItemSummonUnit uiItem = Managers.UI.MakeSubItem<UIItemSummonUnit>(unitSummonPanel.transform);
            if (!string.IsNullOrEmpty(unitItem))
            {
                uiItem.SetName(unitItem);
            }
        }
        
    }

}
