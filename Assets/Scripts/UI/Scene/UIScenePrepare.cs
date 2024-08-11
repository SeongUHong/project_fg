using System;
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
    }

    enum Buttons
    {
        NextStageBtn,
    }

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
            item.SetName(unit.ToString());
            item.SetUnitId((int)unit);
        }

        BindEvent(GetButton((int)Buttons.NextStageBtn).gameObject, LoadGameScene);
    }

    //�� �̵�
    public void LoadGameScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.GameScene);
    }
}
