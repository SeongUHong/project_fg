using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //캔버스 순서
    int _order = 10;

    UIScene _uiScene;

    //실행중인 UI오브젝트
    public UIScene UIScene { get { return _uiScene; } }

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

    //캔버스 초기설정
    //화면에 포시되는 순서를 정의
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

    //게임 내에 표시되는 UI 생성
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

    //Scene UI를 실행
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

    //UI오브젝트에 자식오브젝트를 추가
    public T MakeSubItem<T>(Transform parentTransform = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if(parentTransform != null)
        {
            go.transform.SetParent(parentTransform);
        }

        return Util.GetOrAddComponent<T>(go);
    }

    // 팝업 UI 활성
    public T ShowPopupUI<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public Sprite GetIcon(string path)
    {
        Sprite icon = Managers.Resource.Load<Sprite>($"Arts/Images/Icons/{path}");
        if (icon == null)
        {
            Debug.Log($"Failed to load icon Sprite. path: (Arts/Images/Icons/{path})");
        }
        return icon;
    }

    public Sprite GetUnitIcon(int unitId)
    {
        return GetIcon($"Units/{unitId}");
    }

    public Sprite GetSkillIcon(int skillId)
    {
        return GetIcon($"Skills/{skillId}");
    }

    public void Clear()
    {
    }

}
