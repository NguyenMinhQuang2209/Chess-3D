using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchChess : TouchItem
{
    private MeshRenderer m_render;
    Material firstMat = null;
    private void Start()
    {
        m_render = GetComponent<MeshRenderer>();
    }
    public override void OnTouchAction()
    {
        if (firstMat == null)
        {
            Material[] mat = m_render.materials;
            if (mat != null && mat.Length > 0)
            {
                firstMat = mat[0];
            }
        }
        if (firstMat != null)
        {
            firstMat = new Material(firstMat);
            Color tempColor = firstMat.color;
            tempColor.r = 0.1f;
            m_render.material = firstMat;
        }
    }

    public override void OnTouchOutAction()
    {
        if (firstMat == null)
        {
            Material[] mat = m_render.materials;
            if (mat != null && mat.Length > 0)
            {
                firstMat = mat[0];
            }
        }
        if (firstMat != null)
        {
            firstMat = new Material(firstMat);
            Color tempColor = firstMat.color;
            tempColor.r = 1f;
            m_render.material = firstMat;
        }
    }
}
