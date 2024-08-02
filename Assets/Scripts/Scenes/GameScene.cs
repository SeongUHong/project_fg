using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        _sceneType = Define.Scenes.GameScene;

        Managers.UI.ShowSceneUI<UISceneGame>();

        // ���� ���� �ʱ�ȭ
        Managers.Game.Init();

        // �� ������Ʈ �̸� ����
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        // ���� ����
        Managers.Game.StartSpawningPool(1);

        // �÷��̾� ����
        GameObject player = Managers.Game.InstantiatePlayer();

        // ī�޶� ����
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(player);
    }

    public override void Clear()
    {
    }
}
