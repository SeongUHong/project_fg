using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scenes
    {
        Unknown,
        MainScene,
        PrepareScene,
        GameScene,
    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Attack,
        Skill,
    }

    public enum SceneLocateObject
    {
        UnitSpawnSpot,
        MonsterSpawnSpot,
        MonsterCrystal,
    }

    public enum Layer
    {
        Unit = 7,
        Monster = 8,
        EnemyStaticObject = 9,
    }

    public enum Skill
    {
        Launch,
        Buff,
    }

    public const float DESPAWN_DELAY_TIME = 5.0f;

    // ù ��������ID
    public const int FIRST_STAGE_ID = 1;

    // ���׷��̵� ����Ʈ �ο� ��
    public const int POINT_PER_STAGE = 1;

    // ������ ���� ����
    public const float INCREASE_GAUGE_INTERVAL = 0.1f;

    // ƽ�� ������ ����ġ
    public const float INCREASE_SUMMON_GAUGE_PER_TICK = 1.0f;

    // ��ȯ ������ �ִ�ġ
    public const float MAX_SUMMON_GAUGE = 100.0f;
}
