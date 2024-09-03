using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawningPool : SpawningPool
{
    public Vector3 monsterPos;
    private IEnumerator spawnCoroutine;
    //스텟
    protected Stat _stat;
    protected int _summonedMonsterCount;
    protected override void Init()
    {
        base.Init();
        _summonedMonsterCount = 0;
        //스폰 지점 초기화
        //_spawnPos = Managers.Game.MonsterSpawnPos;

    }

    void Update()
    {
        
        if (_reservedMonsterCount + _monsterCount <= _keepMonsterCount)
        {
            if (_maxMonsterCount == _summonedMonsterCount)
            {
                StopCoroutine(ReserveSpawn());
                return; 
            }
            StartCoroutine(ReserveSpawn());
        }
        
    }

    //예약된 시간 만큼 뒤에 오브젝트 생성
    protected virtual IEnumerator ReserveSpawn()
    {
        
        _reservedMonsterCount++;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));

        //크리스탈 레벨에 따라 다른 용 소환하고싶다
        int cs = Conf.Main.CURRENT_STAGE;
        GameObject monster;
        switch (cs)
        {
            case 1:
                monster = Managers.Game.Spawn(Define.Layer.Monster, "Monsters/Green");
                break;
            case 2:
                monster = Managers.Game.Spawn(Define.Layer.Monster, "Monsters/Blue");
                break;
            case 3:
                monster = Managers.Game.Spawn(Define.Layer.Monster, "Monsters/Purple");
                break;
            case 4:
                monster = Managers.Game.Spawn(Define.Layer.Monster, "Monsters/Red");
                break;

        }
        Managers.Game.CreatePos(Define.Layer.Monster);

        _reservedMonsterCount--;
        _summonedMonsterCount++;
    }


}
