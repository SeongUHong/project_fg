using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ManagerBase
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        //오브젝트에 Poolable 붙여서 생성
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return Util.GetOrAddComponent<Poolable>(go);
        }

        //스택에 비활성화 후 보존
        public void Push(Poolable poolable)
        {
            if (poolable == null)
            {
                return;
            }

            poolable.transform.parent = Root;
            //비활성화
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            //스택에 보존
            _poolStack.Push(poolable);
        }

        //스택에서 꺼내옴
        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if(_poolStack.Count > 0)
            {
                poolable = _poolStack.Pop();
            }
            else
            {
                poolable = Create();
            }

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad 해제 용도
            if(parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public override void Init()
    {
        //풀링 오브젝트를 위해서 삭제되지 않는 부모 오브젝트를 만듦
        if(_root == null)
        {
            _root = new GameObject { name = "@PoolRoot" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }
    
    //게임오브젝트를 딕셔너리에 보존
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    //_pool딕셔너리에서 관리중인 풀링 오브젝트라면 _poolStack에 넣음
    //관리중이 아니면 파괴
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    //_pool딕셔너리에서 관리중인 풀링 오브젝트라면 _poolStack에서 꺼냄
    //관리중이 아니면 새로 관리 대상으로 만듦
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if(_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    //_pool딕셔너리에서 관리중인 오브젝트를 가져옴
    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false) return null;
        return _pool[name].Original;

    }

    //관리중인 풀링 오브젝트 전부 삭제
    public void Clear()
    {
        foreach(Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }

        _pool.Clear();
    }


}
