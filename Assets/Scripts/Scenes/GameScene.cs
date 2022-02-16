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

        //�� ����, �Ʊ� ������ ���� �Ŵ������� ��� ������ �� �ֵ��� ������ ����
        //GameObject enemy = new GameObject(name = "EnemySpawningPool");
        //UnitSpawningPool enemySpawningPool = Util.GetOrAddComponent<UnitSpawningPool>(enemy);
        //enemySpawningPool.SetKeepEnemyCount(6);

        //�� ������Ʈ �̸� ����
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        //�÷��̾� ����
        GameObject player = Managers.Game.InstantiatePlayer();

        //ī�޶� ����
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(player);
    }

    public override void Clear()
    {
    }
}
