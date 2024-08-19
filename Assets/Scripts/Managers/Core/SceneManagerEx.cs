using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    //SceneManager��� API�� �̹� �����ϱ� ������
    //SceneManagerEx��� �̸����� ����

    public BaseScene CurrentScene
    {
        get
        {
            //������ Ÿ��(BaseScene)�� ����ִ� ������Ʈ�� ã����
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

        // ���� ���� ��쿡�� Stage���� ������ ���� �ҷ���
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
