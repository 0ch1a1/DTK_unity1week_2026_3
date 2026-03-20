using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[ExecuteAlways]
public class Ochiai_EnemyMove_Script : MonoBehaviour
{
    [SerializeField] private  SplineContainer spline;
    [SerializeField] private GameObject followObject;
    private float t;
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    private float SplineLength;

    public bool cautionFlag;

    public Transform targetTrans;
    private Transform startEnemyTrans;

    private bool cationInitialize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveForCaution()
    {
        if (!cationInitialize)
        {
            startEnemyTrans = transform;
            cationInitialize = true;
        }
    }

    private void MoveOnSpline()
    {
        //Splineや追従オブジェクトの失効などを検知してエラー防止
        if (!spline || !followObject) return;
        if (spline.CalculateLength() == 0f) return;

        SplineLength = spline.CalculateLength();
        distance += speed / Time.deltaTime;
        t = distance / SplineLength;

        //t値のクランプ
        t = math.saturate(t);

        // Splineの計算をする核心部分
        spline[0].Evaluate(t, out float3 pos, out float3 tangent, out float3 up);

        // 位置の反映
        followObject.transform.position = (Vector3)pos;

        // 回転の反映
        if (math.any(tangent))
        {
            followObject.transform.rotation = Quaternion.LookRotation((Vector3)tangent, (Vector3)up);
        }
    }
}
