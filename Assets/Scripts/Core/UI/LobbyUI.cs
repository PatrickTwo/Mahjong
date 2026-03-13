using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// 
/// </summary>
public class LobbyUI : BasePageUI<LobbyUIRequestHandler>
{
    protected override void Awake()
    {
        base.Awake();
        requestHandler = new LobbyUIRequestHandler();
    }
}
