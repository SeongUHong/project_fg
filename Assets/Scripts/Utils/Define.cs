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
        EndingScene,
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

    public const float DESPAWN_DELAY_TIME = 5.0f;

    // 첫 스테이지ID
    public const int FIRST_STAGE_ID = 1;

    // 업그레이드 포인트 부여 수
    public const int POINT_PER_STAGE = 1;

    // 최소 플레이어 레벨
    public const int MIN_PLAYER_LEVEL = 1;

    // 게이지 증가 간격
    public const float INCREASE_GAUGE_INTERVAL = 0.1f;

    // 틱당 게이지 증가치
    public const float INCREASE_SUMMON_GAUGE_PER_TICK = 0.5f;

    // 소환 게이지 최대치
    public const float MAX_SUMMON_GAUGE = 100.0f;

    // 캐릭터 소환에 필요한 게이지
    public const float SUMMON_COST = 30.0f;

    // 최소 데미지
    public const int MIN_DAMAGE = 1;
}
