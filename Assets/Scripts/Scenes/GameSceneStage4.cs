using UnityEngine;

public class GameSceneStage4 : BaseScene
{
    GameScene_Panel start_Panel;
    //유지하고싶은 몬스터수
    int keep;
    //실제 적용수치계산 = 몬스터수-1
    int monsters;
    protected override void Init()
    {
        base.Init();
        Managers.Game.Init();

        _sceneType = Define.Scenes.GameSceneStage4;

        Managers.UI.ShowSceneUI<UISceneGame>();

        //적 생산, 아군 생산을 게임 매니저에서 모두 수행할 수 있도록 개선해 보자
        GameObject enemy = new GameObject(name = "EnemySpawningPool");
        //UnitSpawningPool enemySpawningPool = Util.GetOrAddComponent<UnitSpawningPool>(enemy);
        //enemySpawningPool.SetKeepEnemyCount(3);
        GameObject go = new GameObject() { name = "MonsterSpawningPool" };
        MonsterSpawningPool MonsterSpawningPool = Util.GetOrAddComponent<MonsterSpawningPool>(go);
        keep = 2;
        monsters = keep - 1;
        MonsterSpawningPool.SetKeepMonsterCount(monsters);

        //씬 오브젝트 이름 변경
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        start_Panel = Managers.Game.StartPanel;
        start_Panel.Show();



    }

    public override void Clear()
    {
    }
}
