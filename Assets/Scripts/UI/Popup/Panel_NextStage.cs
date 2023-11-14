using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Panel_NextStage : UIBase
{

    enum Buttons
    {
        next_btn,
        main_btn,
    }
    private void Awake()
    {
        transform.gameObject.SetActive(false); // 게임이 시작되면 NestStage 팝업 창을 보이지 않도록 한다.
    }

    public void Show()
    {
        new WaitForSeconds(Define.NEXT_DELAY_TIME);
        transform.gameObject.SetActive(true);
    }

    public void OnClick_Next() // '다음 스테이지로' 버튼을 클릭하며 호출 되어질 함수
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.GameSceneStage2);

    }
    public void OnClick_Main()
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.MainScene);// 메인씬으로 돌아가기
    }
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        //Awake();

        BindEvent(GetButton((int)Buttons.next_btn).gameObject, (PointerEventData data) => OnClick_Next());
        BindEvent(GetButton((int)Buttons.main_btn).gameObject, (PointerEventData data) => OnClick_Main());
    }
}
