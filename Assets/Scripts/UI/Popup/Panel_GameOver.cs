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
        transform.gameObject.SetActive(false); // ������ ���۵Ǹ� GameOver �˾� â�� ������ �ʵ��� �Ѵ�.
    }

    public void Show()
    {
        new WaitForSeconds(Define.RETRY_DELAY_TIME);
        transform.gameObject.SetActive(true);
    }

    public void OnClick_Retry() // '�絵��' ��ư�� Ŭ���ϸ� ȣ�� �Ǿ��� �Լ�
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        scene.LoadScene(Define.Scenes.GameScene); // SceneManager�� LoadScene �Լ��� ����Ͽ�! ���� �� 'GameScene'�� �ٽ� �ҷ������� ��Ų��.
        // ���� ���� �ٽ� �ҷ����� ������ ����� �ȴ�.

    }
    public void OnClick_Main()
    {
        Awake();
        SceneManagerEx scene = Managers.Scene;
        Managers.CurrentStage = 1;
        scene.LoadScene(Define.Scenes.MainScene);// ���ξ����� ���ư���
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Awake();

        BindEvent(GetButton((int)Buttons.retry_btn).gameObject, (PointerEventData data) => OnClick_Retry());
        BindEvent(GetButton((int)Buttons.main_btn).gameObject, (PointerEventData data) => OnClick_Main());
    }

}
