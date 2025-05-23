using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("General Refernces:")]
    public GrapplingGun grapplingGun;
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int percision = 40;
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;
    [SerializeField] [Range(0.001f, 0.01f)] private float GrapplePointPrecision;

    float moveTime = 0;

    public bool isGrappling = true;

    public bool strightLine = true;

    bool setLength = false;

    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = percision;
        waveSize = StartWaveSize;
        strightLine = false;

        LinePointsToFirePoint();

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    void DrawRope()
    {
        if (!strightLine)
        {
            if (Mathf.Sqrt(Mathf.Pow(m_lineRenderer.GetPosition(percision - 1).x - grapplingGun.grapplePoint.x, 2)) <= GrapplePointPrecision)
            {
                strightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
            setLength = false;
        }
        else
        {
            
            
            

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isGrappling = true;
            }
            if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                isGrappling = false;
            }
            if(isGrappling)
            {
                grapplingGun.Grapple();
            }
            else
            {
                var pos = grapplingGun.gunHolder.position;
                grapplingGun.m_springJoint2D.distance = (grapplingGun.grapplePoint - new Vector2(pos.x, pos.y)).magnitude;
            }
            if(!setLength)
            {
               grapplingGun.Grapple();
               setLength = true; 
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }

    void DrawRopeWaves()
    {
        for (int i = 0; i < percision; i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }
}

