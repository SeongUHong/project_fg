using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopupGameClear : UIPopup
{
    enum Buttons
    {
        EndingSceneBtn,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.EndingSceneBtn).gameObject, (PointerEventData data) => LoadEndingScene(data));
    }

    // 준비 씬으로 이동
    public void LoadEndingScene(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scenes.EndingScene);
    }
}
