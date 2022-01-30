using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //ĵ���� ����
    int _order = 10;

    //������ UI������Ʈ
    UIScene _uiScene = null;

    public UIScene UIScene
    {
        get { return _uiScene; }
    }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UIRoot");
            if(root == null)
            {
                root = new GameObject { name = "@UIRoot" };
            }
            return root;
        }
    }

    //ĵ���� �ʱ⼳��
    //ȭ�鿡 ���õǴ� ������ ����
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    //���� ���� ǥ�õǴ� UI ����
    public T MakeWorldUI<T>(Transform parentTransform = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (parentTransform != null)
        {
            go.transform.SetParent(parentTransform);
        }

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
        
     }

    //Scene UI�� ����
    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        if(go == null)
        {
            Debug.Log("Can't Load UI Prefab");
            return null;
        }
        T uiScene = Util.GetOrAddComponent<T>(go);
        _uiScene = uiScene;

        go.transform.SetParent(Root.transform);

        return uiScene;

    }

    public void Clear()
    {
        _uiScene = null;
    }

}
