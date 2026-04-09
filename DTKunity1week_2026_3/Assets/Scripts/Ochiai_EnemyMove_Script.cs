using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Cysharp.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using System.Threading.Tasks;

[ExecuteAlways]
public class Ochiai_EnemyMove_Script : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineAnimate;
    //[SerializeField] private  SplineContainer spline;
    //[SerializeField] private GameObject followObject;
    //private float t;
    //private float distance = 0f;
    [Header("移動速度")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float RotateSpeed;
    //private float SplineLength;

    public bool cautionFlag;

    //private Vector3 currentTargetPos;
    public Vector3 cautionPos;
    private Vector3 startEnemyPos;
    private Quaternion startEnemyRot;

    [Header("反応して向き直るまでの時間")]
    [SerializeField] private float cationSearchSec;
    //[Header("目的地点についてから引き返すまでの時間")]
    //[SerializeField] private float cationWaitSec;

    private int waitTime; 

    private float timer = 0; 

    //private float cautionMoveDis;

    private bool cautionInitialize;
    private bool waitInitialize;

    public MovingState _currentState;
    private MovingState nextState;

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

    private async Task MoveManager()
    {
        switch (_currentState)
        {
            case MovingState.Wait:
                WaitMoving(waitTime);
                break;
            case MovingState.Chase:
                MoveForTarget(cautionPos);
                break;
            case MovingState.Patrol:
                MoveOnSpline();
                break;
            case MovingState.Return:
                MoveForTarget(startEnemyPos);
                await UniTask.WaitUntil(() => Vector3.Distance(transform.position, startEnemyPos) < 0.05f);
                _currentState = MovingState.Patrol;
                break;

        }
    }

    private async Task MoveForTarget(Vector3 targetPos)
    {
        Vector3 planeSelf = Vector3.ProjectOnPlane(transform.forward, transform.up);
        Vector3 planeTarget = Vector3.ProjectOnPlane(transform.position - targetPos, transform.up);

        float signedAngle = Vector3.SignedAngle(planeSelf, planeTarget, transform.up);

        // ターゲットへの方向を計算
        Vector3 direction = (planeTarget - planeSelf).normalized;

        if (direction != Vector3.zero)
        {
            // 現在の向きからターゲットの向きへ少しずつ回転
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotateSpeed);
        }

        await UniTask.WaitUntil(() => Mathf.Abs(signedAngle) < 0.01f);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed);
    }

    private void WaitMoving(int waitMilliSec)
    {
        UniTask.Delay(waitMilliSec).ContinueWith(() =>
        {
            _currentState = nextState;
        }
         ).Forget();

        timer -= Time.deltaTime;

        //Vector3 selfPos = new Vector3(transform.position.x, 0, transform.position.z);
        //Vector3 targetPos = new Vector3(cautionPos.x, 0, cautionPos.z);

        //// ターゲットへの方向を計算
        //Vector3 direction = (targetPos - selfPos).normalized;
        
        //if(timer > waitMilliSec / 2)
        //{
        //    if (direction != Vector3.zero)
        //    {
        //        // 現在の向きからターゲットの向きへ少しずつ回転
        //        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotateSpeed);
        //    }
        //}
        //else if(timer < waitMilliSec / 2)
        //{
            
        //    transform.rotation = Quaternion.Slerp(transform.rotation, startEnemyRot, Time.deltaTime * RotateSpeed);
        //}

        


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
            splineAnimate.enabled = true;
            _currentState = MovingState.Patrol;

        } 
    }

    private void MoveOnSpline()
    {


        if (!splineAnimate.enabled)
        {
            splineAnimate.enabled = true;
        }

        if (cautionFlag)
        {
            waitTime = cationSearchSec;
            splineAnimate.enabled = false;
            //changeState = MovingState.Chase;
            startEnemyPos = transform.position; 
            startEnemyRot = transform.rotation;
            _currentState = MovingState.Wait;
            cautionFlag = false;
        }

        
    }
}
public enum MovingState
{
    Wait,
    Patrol,
    Chase,
    Return
}
