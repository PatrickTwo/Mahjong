using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePageUI<T> : BaseUI where T : BasePageUIRequestHandler
{
    protected T requestHandler;

}
