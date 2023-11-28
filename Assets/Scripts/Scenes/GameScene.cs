using UnityEngine;

public class GameScene : BaseScene
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

        _sceneType = Define.Scenes.GameScene;

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

/*        panel = new GameObject() { name = "Panel_GameOver" };
        panel_GameOver = Util.GetOrAddComponent<Panel_GameOver>(panel);*/


        //�� ������Ʈ �̸� ����
        gameObject.name = System.Enum.GetName(typeof(Define.Scenes), _sceneType);

        start_Panel = Managers.Game.StartPanel;
        start_Panel.Show();

/*        //�÷��̾� ����
        GameObject player = Managers.Game.InstantiatePlayer();

        //ī�޶� ����
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject).SetPlayer(player);*/


    }

    public override void Clear()
    {
    }
}
