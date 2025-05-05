using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class cameraOffseter : MonoBehaviour
{
    [SerializeField] private AnimationCurve offsetX;
    [SerializeField] private AnimationCurve offsetY;
    [SerializeField] private GameObject splineGameObject;
    [SerializeField] private float offsetMultiplier;
    [SerializeField] private float maxMoveSpeed;
    private SplineContainer spline;
    [SerializeField] private int accuracy;
    void Start()
    {
        spline = splineGameObject.GetComponent<SplineContainer>();
        accuracy = accuracy * spline.Spline.Count;
    }
    void Update()
    {
        float splinePos = GetClosestTOnSpline(spline,transform.position,accuracy) * spline.Spline.Count;
        var CinemachineFollow = GetComponent<CinemachineFollow>();
        var offset = new Vector3(offsetX.Evaluate(splinePos)*offsetMultiplier,offsetY.Evaluate(splinePos)*offsetMultiplier,CinemachineFollow.FollowOffset.z);
        CinemachineFollow.FollowOffset = Vector3.MoveTowards(CinemachineFollow.FollowOffset, offset, maxMoveSpeed*Time.deltaTime);
    }

    private float GetClosestTOnSpline(SplineContainer spline, Vector3 target, int sampleCount)
    {
        float closestDistance = float.MaxValue;
        float closestT = 0;

        for (int i = 0; i <= sampleCount; i++)
        {
            float t = i / (float)sampleCount; // Normalized position along the spline (0 to 1)
            Vector3 pointOnSpline = spline.EvaluatePosition(t);
            float distance = Vector3.Distance(target, pointOnSpline);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }
}
