using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawningPool : SpawningPool
{
    public Vector3 monsterPos;

    //����
    protected Stat _stat;

    protected override void Init()
    {
        base.Init();
        //���� ���� �ʱ�ȭ
        //_spawnPos = Managers.Game.MonsterSpawnPos;

    }

    void Update()
    {

        while (_reservedMonsterCount + _monsterCount <= _keepMonsterCount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    //����� �ð� ��ŭ �ڿ� ������Ʈ ����
    protected virtual IEnumerator ReserveSpawn()
    {
        _reservedMonsterCount++;
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));

        //ũ����Ż ������ ���� �ٸ� �� ��ȯ�ϰ�ʹ�

        GameObject monster = Managers.Game.Spawn(Define.Layer.Monster, "Monsters/Purple");
        Managers.Game.CreatePos(Define.Layer.Monster);

        _reservedMonsterCount--;
    }


}
