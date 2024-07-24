using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Map")]
public class MapScriptableObject : ScriptableObject
{
    public Texture2D mapTexture;
    public Texture2D chessTexture;
}
