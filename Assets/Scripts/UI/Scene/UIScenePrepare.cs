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
    }

    enum Texts
    {
        StageIdTxt,
        UpgradePoint,
    }

    enum Buttons
    {
        NextStageBtn,
    }

    List<UIItemUnitUpgrade> _upgradeItemUIs = new List<UIItemUnitUpgrade>();

    public override void Init()
    {
        Bind<GameObject>(typeof(Objects));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        // ��������ID ���
        Text stageIdTxt = Get<Text>((int)Texts.StageIdTxt);
        stageIdTxt.text = $"Stage{Managers.Status.StageId}";

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

        BindEvent(GetButton((int)Buttons.NextStageBtn).gameObject, LoadGameScene);

        // ��ų ����Ʈ
        UpdatePoint();
    }

    //�� �̵�
    public void LoadGameScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.GameScene);
    }

    // ���׷��̵带 �������� ���� ó��
    public void ExecUpgrade()
    {
        UpdatePoint();
        UpdateSubItemUIs();
    }

    // ��ų ����Ʈ ǥ�� ����
    public void UpdatePoint()
    {
        Get<Text>((int)Texts.UpgradePoint).text = Managers.Status.Point.ToString();
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
