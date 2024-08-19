using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISceneEnding : UIScene
{
    enum Buttons
    {
        MainSceneBtn,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.MainSceneBtn).gameObject, LoadPrePareScene);
    }

    //æ¿ ¿Ãµø
    public void LoadPrePareScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.MainScene);
    }
}
