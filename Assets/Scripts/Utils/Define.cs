using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scenes
    {
        Unknown,
        MainScene,
        GameSceneStage1,
        GameSceneStage2,
        GameSceneStage3,
        GameSceneStage4,

    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Attack,
        Skill,
        Clear,
    }

    public enum UIEvent
    {
        Click,
        Drag,
        Press,
        PointerDown,
        PointerUp,
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
        Player = 10,
    }

    public enum Skill
    {
        Launch,
        Buff,
    }

    public const float DESPAWN_DELAY_TIME = 1.0f;
    public const float RETRY_DELAY_TIME = 2.0f;
    public const float NEXT_DELAY_TIME = 1.0f;
}
