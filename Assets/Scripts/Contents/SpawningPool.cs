using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class SpawningPool : MonoBehaviour
{
    //유지할 오브젝트의 수
    [SerializeField]
    protected int _keepObjectCount = 0;
    //스폰 장소
    [SerializeField]
    protected Vector3 _spawnPos;
    //스폰 텀
    [SerializeField]
    protected float _spawnTime = 5.0f;

    //생성 예약된 오브젝트
    protected int _spawnedCount = 0;

    private void Start()
    {
        Init();
    }
    
    protected virtual void Init()
    {
        // 스폰 지점 초기화
        _spawnPos = SpawnPos();

        // 스폰 시작
        StartCoroutine(ReserveSpawn());
    }

    public void SetKeepEnemyCount(int count)
    {
        _keepObjectCount = count;
    }

    // 예약된 시간 만큼 뒤에 오브젝트 생성
    protected virtual IEnumerator ReserveSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));

            // 상한까지 소환된 경우 스킵
            if (Managers.Game.MonsterNum >= _keepObjectCount)
                continue;

            Managers.Game.Spawn(CharacterPath());
        }
    }

    protected abstract Vector3 SpawnPos();
    protected abstract string CharacterPath();
}
