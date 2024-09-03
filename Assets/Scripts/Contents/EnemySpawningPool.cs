using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningPool : SpawningPool
{
    protected override void Init()
    {
        base.Init();
        //스폰 지점 초기화
        _spawnPos = Managers.Game.UnitSpawnPos;


    }

    void Update()
    {
        if (_reserveCount + _unitCount <= _keepObjectCount)
        {
            //StartCoroutine(ReserveSpawn());
        }
        else
        {
            StopCoroutine(ReserveSpawn());
        }
    }

    //예약된 시간 만큼 뒤에 오브젝트 생성
    protected virtual IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));

        GameObject character = Managers.Game.Spawn(Define.Layer.Monster, "Units/FitnessGirlSniper");
        character.transform.position = Managers.Game.CreatePos(Define.Layer.Monster);
        _reserveCount--;
        
    }
}
