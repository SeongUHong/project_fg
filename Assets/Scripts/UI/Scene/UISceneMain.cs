using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISceneMain : UIScene
{
    enum Buttons
    {
        LoadGameSceneBtn,
    }

    enum Texts
    {
        LoadGameSceneTxt,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        BindEvent((GetButton((int)Buttons.LoadGameSceneBtn).gameObject), LoadGameScene);
    }

    //æ¿ ¿Ãµø
    public void LoadGameScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.GameSceneStage1);
    }
}
