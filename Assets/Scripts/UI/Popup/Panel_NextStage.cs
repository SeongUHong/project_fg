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
        transform.gameObject.SetActive(false); // ������ ���۵Ǹ� NestStage �˾� â�� ������ �ʵ��� �Ѵ�.
    }

    public void Show()
    {
        new WaitForSeconds(Define.NEXT_DELAY_TIME);
        transform.gameObject.SetActive(true);
    }

    public void OnClick_Next() // '���� ����������' ��ư�� Ŭ���ϸ� ȣ�� �Ǿ��� �Լ�
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.GameSceneStage2);

    }
    public void OnClick_Main()
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.MainScene);// ���ξ����� ���ư���
    }
    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        //Awake();

        BindEvent(GetButton((int)Buttons.next_btn).gameObject, (PointerEventData data) => OnClick_Next());
        BindEvent(GetButton((int)Buttons.main_btn).gameObject, (PointerEventData data) => OnClick_Main());
    }
}
