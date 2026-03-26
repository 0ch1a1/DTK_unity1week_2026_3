using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

[ExecuteAlways]
public class Ochiai_EnemyMove_Script : MonoBehaviour
{
    //[SerializeField] private SplineAnimate splineAnimate;
    //[SerializeField] private  SplineContainer spline;
    //[SerializeField] private GameObject followObject;
    //private float t;
    //private float distance = 0f;
    //[SerializeField] private float MoveSpeed;
    [SerializeField] private float RotateSpeed;
    //private float SplineLength;

    public bool cautionFlag;

    //private Vector3 currentTargetPos;
    public Vector3 cautionPos;
    //private Transform startEnemyTrans;
    private Quaternion startEnemyRot;

    [Header("反応して向き直るまでの時間")]
    [SerializeField] private float cationSearchSec;
    //[Header("目的地点についてから引き返すまでの時間")]
    //[SerializeField] private float cationWaitSec;

    private float waitTime; 

    private float timer = 0; 

    //private float cautionMoveDis;

    private bool cautionInitialize;
    private bool waitInitialize;

    public MovingState _currentState;
    //private MovingState changeState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cautionInitialize = false;
        waitInitialize = false;
        cautionFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveManager();
    }

    private void MoveManager()
    {
        switch (_currentState)
        {
            case MovingState.Wait:
                WaitMoving(waitTime);
                break;
            //case MovingState.Chase:
            //    MoveForTarget(currentTargetPos);
            //    break;
            case MovingState.Patrol:
                MoveOnSpline();
                break;

        }
    }

    //private void MoveForTarget(Vector3 targetPos)
    //{
    //    if (waitInitialize)
    //    {
    //        waitInitialize = false;
    //    }

    //    if (Vector3.Distance(targetPos, transform.position) < 0.1f)
    //    {
    //        waitTime = cationWaitSec;
    //        if (changeState == MovingState.Patrol)
    //        {
    //            splineAnimate.enabled = true;
    //            _currentState = MovingState.Patrol;
    //        }
    //        else if(changeState == MovingState.Chase)
    //        {
    //            changeState = MovingState.Patrol;
    //            _currentState = MovingState.Wait;
    //        }
            
    //    }



    //    // ターゲットへの方向を計算
    //    Vector3 direction = (targetPos - transform.position).normalized;
    //    if (direction != Vector3.zero)
    //    {
    //        // 現在の向きからターゲットの向きへ少しずつ回転
    //        Quaternion lookRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotateSpeed);
    //    }

    //    // 前方に移動
    //    transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

    //}

    private void WaitMoving(float waitSec)
    {
        timer -= Time.deltaTime;
        if (!cautionInitialize)
        {
            startEnemyRot = transform.rotation;

            cautionInitialize = true;
        }

        if (!waitInitialize)
        {
            timer = waitSec;

            waitInitialize = true;
        }

        Vector3 selfPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(cautionPos.x, 0, cautionPos.z);

        // ターゲットへの方向を計算
        Vector3 direction = (targetPos - selfPos).normalized;
        
        if(timer > waitSec / 2)
        {
            if (direction != Vector3.zero)
            {
                // 現在の向きからターゲットの向きへ少しずつ回転
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotateSpeed);
            }
        }
        else if(timer < waitSec / 2)
        {
            
            transform.rotation = Quaternion.Slerp(transform.rotation, startEnemyRot, Time.deltaTime * RotateSpeed);
        }

        


        if (timer < 0)
        {
            //if(changeState == MovingState.Patrol)
            //{
            //    currentTargetPos = startEnemyTrans.position;
            //}
            //else if(changeState == MovingState.Chase)
            //{
            //    currentTargetPos = cautionPos;
            //}
            //    _currentState = MovingState.Chase;
            //splineAnimate.enabled = true;
            _currentState = MovingState.Patrol;

        } 
    }

    private void MoveOnSpline()
    {
        if (cautionInitialize)
        {
            cautionInitialize = false;
        }

        if (waitInitialize)
        { 
            waitInitialize = false;
        }

        if (cautionFlag)
        {
            waitTime = cationSearchSec;
            //splineAnimate.enabled = false;
            //changeState = MovingState.Chase;
            _currentState = MovingState.Wait;
            cautionFlag = false;
        }

        ////Splineや追従オブジェクトの失効などを検知してエラー防止
        //if (!spline || !followObject) return;
        //if (spline.CalculateLength() == 0f) return;

        //SplineLength = spline.CalculateLength();
        //distance += MoveSpeed / 3.6f * Time.deltaTime;
        //t = distance / SplineLength;

        ////t値のクランプ
        //t = math.saturate(t);

        //// Splineの計算をする核心部分
        //spline[0].Evaluate(t, out float3 pos, out float3 tangent, out float3 up);

        //// 位置の反映
        //followObject.transform.position = (Vector3)pos;

        //// 回転の反映
        //if (math.any(tangent))
        //{
        //    followObject.transform.rotation = Quaternion.LookRotation((Vector3)tangent, (Vector3)up);
        //}
    }
}
public enum MovingState
{
    Wait,
    Patrol,
    Chase
}
