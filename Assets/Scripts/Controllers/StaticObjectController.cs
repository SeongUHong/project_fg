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

    public virtual void Init() {}
    public virtual void Destroy(){}
}
