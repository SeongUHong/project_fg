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
        transform.gameObject.SetActive(false); // ������ ���۵Ǹ� GameOver �˾� â�� ������ �ʵ��� �Ѵ�.
    }

    public void Show()
    {
        new WaitForSeconds(Define.RETRY_DELAY_TIME);
        transform.gameObject.SetActive(true);
    }

    public void OnClick_Game_Start() // '�絵��' ��ư�� Ŭ���ϸ� ȣ�� �Ǿ��� �Լ�
    {

        //�÷��̾� ����
        GameObject player = Managers.Game.InstantiatePlayer();

        //ī�޶� ����
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(player);

        Awake();

    }
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.GameStart_btn).gameObject, (PointerEventData data) => OnClick_Game_Start());
    }
}
