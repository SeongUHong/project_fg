using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Panel_GameOver : UIBase
{

    enum Buttons
    {
        retry_btn,
        main_btn,
    }
    private void Awake()
    {
        transform.gameObject.SetActive(false); // 게임이 시작되면 GameOver 팝업 창을 보이지 않도록 한다.
    }

    public void Show()
    {
        new WaitForSeconds(Define.RETRY_DELAY_TIME);
        transform.gameObject.SetActive(true);
    }

    public void OnClick_Retry() // '재도전' 버튼을 클릭하며 호출 되어질 함수
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.GameScene); // SceneManager의 LoadScene 함수를 사용하여! 현재 신 'GameScene'을 다시 불러오도록 시킨다.
        // 같은 신을 다시 불러오면 게임이 재시작 된다.

    }
    public void OnClick_Main()
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        Managers.CurrentStage = 1;
        scene.LoadScene(Define.Scenes.MainScene);// 메인씬으로 돌아가기
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Awake();

        BindEvent(GetButton((int)Buttons.retry_btn).gameObject, (PointerEventData data) => OnClick_Retry());
        BindEvent(GetButton((int)Buttons.main_btn).gameObject, (PointerEventData data) => OnClick_Main());
    }

}
