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
        //_reservedMonsterCount++;
        while (_reservedMonsterCount + _monsterCount <= _keepMonsterCount)
        {
            StartCoroutine(ReserveSpawn());
            //Debug.Log(_reservedMonsterCount + _monsterCount) ;
        }
    }

    //����� �ð� ��ŭ �ڿ� ������Ʈ ����
    protected virtual IEnumerator ReserveSpawn()
    {
        _reservedMonsterCount++;

        yield return new WaitForSeconds(UnityEngine.Random.Range(0, _spawnTime));

        //ũ����Ż ������ ���� �ٸ� �� ��ȯ�ϰ�ʹ�
        float cs = Managers.CurrentStage;
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
    }


}
