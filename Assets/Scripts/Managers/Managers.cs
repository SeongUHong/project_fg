using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    GameManagerEx _game = new GameManagerEx();

    public static GameManagerEx Game { get { return Instance._game; } }
    #endregion

    #region Core
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();

    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    static void Init()
    {
        //�Ŵ��� �ʱ�ȭ & ���� ��� ����
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            //Ǯ �Ŵ��� ����
            s_instance._pool.Init();

            //���� �Ŵ��� ����
            s_instance._game.Init();
        }
    }

    public static void Clear()
    {
        Pool.Clear();
        Scene.Clear();
        UI.Clear();
    }
}
