using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScene : UIBase
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }

}
