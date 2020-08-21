using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererPathMovement : PathMovement
{
    LineRenderer m_lineRenderer;
    Vector3[] m_positions;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();

        m_positions = new Vector3[m_lineRenderer.positionCount];
        m_lineRenderer.GetPositions(m_positions);
    }

    private void Start()
    {
        //m_lineRenderer.enabled = false; // Only need to run this on for debugging
    }

    public override Vector3[] GetPath()
    {
        return m_positions;
    }
}
