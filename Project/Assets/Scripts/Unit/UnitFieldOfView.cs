using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFieldOfView : UnitComponent
{
    [SerializeField] private LayerMask m_targetMask;
    [SerializeField] private LayerMask m_obstacleMask;

    private float m_viewRadius;
    private float m_viewAngle;
    private float m_meshResolution = 10f;
    private int m_edgeResolveIterations = 4;
    private float m_edgeDistanceThreshold = 0.5f;

    private MeshRenderer m_meshRenderer;
    private MeshFilter m_viewMeshFilter;
    private Mesh m_viewMesh;

    //For Editor
    public float ViewRadius { get { return m_viewRadius; } }
    public float ViewAngle { get { return m_viewAngle; } }

    protected override void Start()
    {
        base.Start();

        m_unitView.UnitMovement.UnitMoved += DetectEnemyUnits;

        m_viewRadius = m_unitView.UnitData.viewRadius;
        m_viewAngle = m_unitView.UnitData.viewAngle;

        //Create View Mesh Filter
        GameObject viewMesh = new GameObject("FieldOfView");
        viewMesh.transform.parent = this.transform;
        viewMesh.transform.localPosition = Vector3.zero;

        m_meshRenderer = viewMesh.AddComponent<MeshRenderer>();
        m_viewMeshFilter = viewMesh.AddComponent<MeshFilter>();
        
        m_meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        m_meshRenderer.receiveShadows = false;
        m_meshRenderer.material = m_unitView.UnitData.fieldOfViewMaterial;

        m_viewMesh = new Mesh();
        m_viewMeshFilter.mesh = m_viewMesh;

        DrawFieldOfView();
        DisableFieldOfView();
    }    

    private void DetectEnemyUnits()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_viewRadius, m_targetMask);

        List<UnitView> enemies = new List<UnitView>();

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < m_viewAngle * 0.5f)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, m_obstacleMask))
                {
                    UnitView unit = targetsInViewRadius[i].GetComponent<UnitView>();

                    if(unit.TeamID != m_unitView.TeamID)
                    {
                        enemies.Add(unit);
                    }
                }
            }
        }

        //Send Combat Encounter To Combat Manager
        if(enemies.Count != 0)
        {
            GameManager.Instance.CombatManager.SetupCombatEncounter(enemies,m_unitView);
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(m_viewAngle * m_meshResolution);

        float stepAngleSize = m_viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - m_viewAngle * 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if(i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > m_edgeDistanceThreshold;

                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        m_viewMesh.Clear();
        m_viewMesh.vertices = vertices;
        m_viewMesh.triangles = triangles;
        m_viewMesh.RecalculateNormals();
    }

    public void EnableFieldOfView()
    {
        m_meshRenderer.enabled = true;
    }

    public void DisableFieldOfView()
    {
        m_meshRenderer.enabled = false;
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < m_edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > m_edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position,dir, out hit, m_viewRadius,m_obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * m_viewRadius, m_viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
