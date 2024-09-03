using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScene_Panel : UIBase
{

    enum Buttons
    {
        GameStart_btn,
        main_btn,
    }
    private void Awake()
    {
        transform.gameObject.SetActive(false); // 게임이 시작되면 GameOver 팝업 창을 보이지 않도록 한다.
    }

    public void Show()
    {
        new WaitForSeconds(Define.RETRY_DELAY_TIME);
        Time.timeScale = 0;
        transform.gameObject.SetActive(true);
    }

    public void OnClick_Game_Start() // 'start' 버튼을 클릭하며 호출 되어질 함수
    {

        //플레이어 생성
        //GameObject player = Managers.Game.InstantiatePlayer();


        //카메라 설정
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(Managers.Game.Player);


        Time.timeScale = 1;
        Awake();
    }

    public void OnClick_Main()
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.MainScene);// 메인씬으로 돌아가기

        Conf.Main.CLEAR_FLAG = false;
        Conf.Main.CURRENT_STAGE = 1;
        Conf.Main.CURRENT_SCENE = 1;
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.GameStart_btn).gameObject, (PointerEventData data) => OnClick_Game_Start());
        BindEvent(GetButton((int)Buttons.main_btn).gameObject, (PointerEventData data) => OnClick_Main());

    }
}
