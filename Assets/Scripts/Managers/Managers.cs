using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    GameManagerEx _game = new GameManagerEx();
    SkillManager _skill = new SkillManager();

    public static GameManagerEx Game { get { return Instance._game; } }
    public static SkillManager Skill { get { return Instance._skill; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
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
        //매니저 초기화 & 없을 경우 생성
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

            //풀 매니저 실행
            s_instance._pool.Init();

            //게임 매니저 실행
            s_instance._game.Init();
        }
    }

    public static void Clear()
    {
        Pool.Clear();
        Scene.Clear();
        UI.Clear();
        Resource.Clear();
        Game.Clear();
    }
}
