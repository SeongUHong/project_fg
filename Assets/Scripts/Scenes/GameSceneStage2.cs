using UnityEngine;

public class GameSceneStage2 : BaseScene
{
    GameScene_Panel start_Panel;

    protected override void Init()
    {
        base.Init();
        Managers.Game.Init();

        _sceneType = Define.Scenes.GameSceneStage2;

        Managers.UI.ShowSceneUI<UISceneGame>();

        //적 생산, 아군 생산을 게임 매니저에서 모두 수행할 수 있도록 개선해 보자
        GameObject enemy = new GameObject(name = "EnemySpawningPool");
        //UnitSpawningPool enemySpawningPool = Util.GetOrAddComponent<UnitSpawningPool>(enemy);
        //enemySpawningPool.SetKeepEnemyCount(3);
        GameObject go = new GameObject() { name = "MonsterSpawningPool" };
        //MonsterSpawningPool MonsterSpawningPool = Util.GetOrAddComponent<MonsterSpawningPool>(go);
        //MonsterSpawningPool.SetKeepMonsterCount(2);


        //씬 오브젝트 이름 변경
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        start_Panel = Managers.Game.StartPanel;
        start_Panel.Show();



    }

    public override void Clear()
    {
    }
}
