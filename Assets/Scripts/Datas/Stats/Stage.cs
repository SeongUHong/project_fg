using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
    //엔티티 클래스
    [Serializable]
    public class Stage
    {
        public List<StageSpawnMonster> spawn_monsters;
        public int map_id;
    }

    [Serializable]
    public class StageSpawnMonster
    {
        //몬스터ID CharacterConf.Monster(주키 역할)
        public int monster_id;
        // 몬스터 레벨
        public int level;
        // 소환 상한
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
