using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    //컴포넌트 가져오기 & 없으면 추가시키고 가져오기
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if(component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) return null;
        return transform.gameObject;
    }


    //해당 이름의 자식 오브첵트 찾기
    //recursive가 true일 경우, 자식의 자식까지 검색
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if(recursive == false)
        {
            for(int i=0;i<go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name) return component;
            }
        }

        return null;
    }

    //오브젝트가 존재하는지 확인
    public static bool IsValid(GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
