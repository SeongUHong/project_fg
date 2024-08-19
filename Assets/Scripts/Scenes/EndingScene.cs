using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        _sceneType = Define.Scenes.Ending;

        Managers.UI.ShowSceneUI<UISceneEnding>();
    }

    public override void Clear()
    {
        
    }
}
