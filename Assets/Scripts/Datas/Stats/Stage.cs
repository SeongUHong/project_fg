using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
    //��ƼƼ Ŭ����
    [Serializable]
    public class Stage
    {
        public List<StageSpawnMonster> spawn_monsters;
    }

    [Serializable]
    public class StageSpawnMonster
    {
        //����ID CharacterConf.Monster(��Ű ����)
        public int monster_id;
        // ���� ����
        public int level;
        // ��ȯ ����
        public int limit_num;
    }

    [Serializable]
    public class SpawnMonsterLoader : ILoader<int, StageSpawnMonster>
    {
        public List<StageSpawnMonster> spawn_monsters = new List<StageSpawnMonster>();

        public Dictionary<int, StageSpawnMonster> MakeDict()
        {
            Dictionary<int, StageSpawnMonster> dict = new Dictionary<int, StageSpawnMonster>();

            foreach (StageSpawnMonster mon in spawn_monsters)
            {
                dict.Add(mon.monster_id, mon);
            }

            return dict;
        }
    }

}
