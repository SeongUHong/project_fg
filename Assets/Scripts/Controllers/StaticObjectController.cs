using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticObjectController : MonoBehaviour
{
    protected bool flag = false;

    void Start()
    {
        Init();
    }

    protected virtual void Init() {}
    public void Destroy()
    {
        ControllerConf._clearFlag = true;
        Managers.Resource.Destroy(gameObject);
    }
}
