using UnityEngine;

public class GameSceneStage4 : BaseScene
{
    GameScene_Panel start_Panel;
    //�����ϰ���� ���ͼ�
    int keep;
    //���� �����ġ��� = ���ͼ�-1
    int monsters;
    protected override void Init()
    {
        base.Init();
        Managers.Game.Init();

        _sceneType = Define.Scenes.GameSceneStage4;

        Managers.UI.ShowSceneUI<UISceneGame>();

        //�� ����, �Ʊ� ������ ���� �Ŵ������� ��� ������ �� �ֵ��� ������ ����
        GameObject enemy = new GameObject(name = "EnemySpawningPool");
        //UnitSpawningPool enemySpawningPool = Util.GetOrAddComponent<UnitSpawningPool>(enemy);
        //enemySpawningPool.SetKeepEnemyCount(3);
        GameObject go = new GameObject() { name = "MonsterSpawningPool" };
        MonsterSpawningPool MonsterSpawningPool = Util.GetOrAddComponent<MonsterSpawningPool>(go);
        keep = 2;
        monsters = keep - 1;
        MonsterSpawningPool.SetKeepMonsterCount(monsters);

        //�� ������Ʈ �̸� ����
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        start_Panel = Managers.Game.StartPanel;
        start_Panel.Show();



    }

    public override void Clear()
    {
    }
}
