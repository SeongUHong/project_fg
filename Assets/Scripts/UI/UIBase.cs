
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    //바인드한 UI오브젝트를 보존하는 딕셔너리
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    private void Start()
    {
        Init();
    }

    //UI오브젝트를 바인드함(딕셔너리에 저장하여 UI오브젝트를 Get메서드로 꺼내쓰기 용이하게 함)
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {
            if(typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            }

            if(objects[i] == null)
            {
                if ($"{names[i]}" == "UIHpBar" || $"{names[i]}" == "Panel_GameOver")
                    continue;
                Debug.Log($"Failed to bind {names[i]}");
            }
        }

    }

    //딕셔너리[index]에 저장된 UI오브젝트를 꺼냄
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        //value가 없을 경우false, 있을 경우 value를 out으로 반환
        if (_objects.TryGetValue(typeof(T), out objects) == false) return null;

        return objects[index] as T;

    }

    protected GameObject GetGameObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected Button GetButton(int index) { return Get<Button>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }

    public void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        //해당 UI에 EventHandler 컴포넌트 추가
        UIEventHandler evt = Util.GetOrAddComponent<UIEventHandler>(go);

        //추가한 EventHandler에 액션을 추가
        //이벤트가 두번 추가되어 두번 실행되는 것을 방지하기위해
        //이벤트를 한번 빼고 추가해줌
        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                //드래그 처리
                evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
                break;
        }
    }

}
