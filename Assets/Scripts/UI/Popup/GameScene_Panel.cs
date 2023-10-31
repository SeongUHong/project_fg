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

    public void OnClick_Game_Start() // '재도전' 버튼을 클릭하며 호출 되어질 함수
    {

        //플레이어 생성
        GameObject player = Managers.Game.InstantiatePlayer();

        //카메라 설정
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(player);

        Awake();

    }
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.GameStart_btn).gameObject, (PointerEventData data) => OnClick_Game_Start());
    }
}
