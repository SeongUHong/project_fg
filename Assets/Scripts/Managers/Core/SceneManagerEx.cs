using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    //SceneManager라는 API가 이미 존재하기 때문에
    //SceneManagerEx라는 이름으로 생성

    public BaseScene CurrentScene
    {
        get
        {
            //지정한 타입(BaseScene)을 들고있는 오브젝트를 찾아줌
            return GameObject.FindObjectOfType<BaseScene>();
        }
    }

    public void LoadScene(Define.Scenes type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scenes type)
    {
        string name = null;

        // 게임 씬의 경우에는 Stage마다 설정된 맵을 불러옴
        if (type == Define.Scenes.GameScene)
        {
            name = $"{System.Enum.GetName(typeof(Define.Scenes), type)}{Managers.Data.GetStageMapIdByStageId(Managers.Status.StageId)}";
        }
        else
        {
            name = System.Enum.GetName(typeof(Define.Scenes), type);
        }

        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
