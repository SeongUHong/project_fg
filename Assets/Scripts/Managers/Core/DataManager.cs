using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILoader<key, value>
{
    Dictionary<key, value> MakeDict();
}

public class DataManager
{
    const string ROOT_DIRECTORY = "Datas/";
    const string STAT_DIRECTORY = ROOT_DIRECTORY + "Stats/";
    const string STAGE_DIRECTORY = ROOT_DIRECTORY + "Stages/";
    const string SKILL_DIRECTORY = ROOT_DIRECTORY + "Skills/";

    public string GetJsonText(string path)
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.Log($"Not Exist Json File : {path}");
            return null;
        }
        return textAsset.text;
    }

    Loader LoadJson<Loader, key, value>(string path) where Loader : ILoader<key, value>
    {
        return JsonUtility.FromJson<Loader>(GetJsonText(path));
    }

    public data.Stat GetStatByLevel(string path, int level)
    {
        Dictionary<int, data.Stat> statDict = LoadJson<data.StatLoader, int, data.Stat>(STAT_DIRECTORY + path).MakeDict();
        if (statDict.Count == 0)
        {
            Debug.Log($"Not exist data : {STAT_DIRECTORY + path}");
            return null;
        }

        return statDict[level];
    }

    // 레벨별 플레이어 Stat
    public Dictionary<int, data.Stat> GetPlayerStatDict()
    {
        string path = "PlayerStat";
        Dictionary<int, data.Stat> statDict = LoadJson<data.StatLoader, int, data.Stat>(STAT_DIRECTORY + path).MakeDict();
        if (statDict.Count == 0)
        {
            Debug.Log($"Not exist data : {STAT_DIRECTORY + path}");
            return null;
        }

        return statDict;
    }

    public int GetPlayerMaxLevel()
    {
        Dictionary<int, data.Stat> statDict = GetPlayerStatDict();
        return statDict.Keys.Max();
    }

    public Dictionary<int, data.Stat> GetUnitStatDic(int unitId)
    {
        string unitName = ((CharacterConf.Unit)unitId).ToString();
        Dictionary<int, data.Stat> statDict = LoadJson<data.StatLoader, int, data.Stat>(STAT_DIRECTORY + unitName + "Stat").MakeDict();
        if (statDict.Count == 0)
        {
            Debug.Log($"Not exist data : {STAT_DIRECTORY + unitName}");
            return null;
        }

        return statDict;
    }

    public data.Stat GetUnitStatByLevel(int unitId, int level)
    {
        Dictionary<int, data.Stat> statDict = GetUnitStatDic(unitId);
        return statDict[level];
    }

    public int GetUnitMaxLevel(int unitId)
    {
        Dictionary<int, data.Stat> statDict = GetUnitStatDic(unitId);
        return statDict.Keys.Max();
    }

    // 스테이지ID로 스테이지를 취득
    public data.Stage GetStageByStageId(int stageId)
    {
        data.Stage stage = JsonUtility.FromJson<data.Stage>(GetJsonText(STAGE_DIRECTORY + stageId));

        if (stage == null)
        {
            Debug.Log($"Not Exist Json File : {STAGE_DIRECTORY + stageId}");
            return null;
        }

        return stage;
    }

    // 스테이지ID로 맵ID 취득
    public int GetStageMapIdByStageId(int stageId)
    {
        return GetStageByStageId(stageId).map_id;
    }

    // 스테이지ID로 스테이지 몬스터 정보를 취득
    public List<data.StageSpawnMonster> GetStageSpawnMonsterByStageId(int stageId)
    {
        data.Stage stage = GetStageByStageId(stageId);
        return stage.spawn_monsters;
    }

    // 스테이지ID로 스테이지 몬스터 정보를 취득
    public data.StageSpawnMonster GetSpawnMonsterByStageIdAndMonsterId(int stageId, int monsterId)
    {
        Dictionary<int, data.StageSpawnMonster> monsterDict = LoadJson<data.SpawnMonsterLoader, int, data.StageSpawnMonster>(STAGE_DIRECTORY + stageId).MakeDict();

        if (monsterDict.Count == 0)
        {
            Debug.Log($"Not exist data : {STAGE_DIRECTORY + stageId}");
            return null;
        }

        return monsterDict[monsterId];
    }

    // 마지막 스테이지 취득
    public int GetFinalStageId()
    {
        // Stage폴더 안의 모든 파일 중 가장 수치가 높은 파일명
        List<int> stageIds = new List<int>();
        foreach(Object file in Resources.LoadAll(STAGE_DIRECTORY))
        {
            stageIds.Add(int.Parse(file.name));
        }

        return stageIds.Max();
    }

    // 스킬ID와 레벨로 스킬정보 취득
    public Dictionary<int, data.Skill> GetSKillDict(int skillId)
    {
        string skillName = Util.NumToEnumName<SkillConf.Skill>(skillId);
        Dictionary<int, data.Skill> skillDict = LoadJson<data.SkillLoader, int, data.Skill>(SKILL_DIRECTORY + skillName).MakeDict();
        if (skillDict.Count == 0)
        {
            Debug.Log($"Not exist data : {SKILL_DIRECTORY + skillName}");
            return null;
        }

        return skillDict;
    }

    // 스킬ID와 레벨로 스킬정보 취득
    public data.Skill GetSKillBySkillIdAndLevel(int skillId, int level)
    {
        Dictionary<int, data.Skill> skillDict = GetSKillDict(skillId);
        return skillDict[level];
    }

    public int GetSkillMaxLevel(int skillId)
    {
        Dictionary<int, data.Skill> skillDict = GetSKillDict(skillId);
        return skillDict.Keys.Max();
    }
}
