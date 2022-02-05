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

        //적 생산, 아군 생산을 게임 매니저에서 모두 수행할 수 있도록 개선해 보자
        GameObject enemy = new GameObject(name = "EnemySpawningPool");
        UnitSpawningPool enemySpawningPool = Util.GetOrAddComponent<UnitSpawningPool>(enemy);
        enemySpawningPool.SetKeepEnemyCount(6);

        //씬 오브젝트 이름 변경
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        //플레이어 생성
        GameObject player = Managers.Game.InstantiatePlayer();

        //카메라 설정
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(player);
    }

    public override void Clear()
    {
    }
}
