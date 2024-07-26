using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchItem : MonoBehaviour
{
    public abstract void OnTouchAction();
    public abstract void OnTouchOutAction();
}
