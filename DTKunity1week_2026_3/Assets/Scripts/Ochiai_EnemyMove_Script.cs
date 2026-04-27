using UnityEngine;
using UnityEngine.Splines;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

[ExecuteAlways]
public class Ochiai_EnemyMove_Script : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineAnimate;

    [Header("動かすオブジェクト")]
    [SerializeField] private GameObject selfObj;
    //private float t;
    //private float distance = 0f;
    [Header("移動速度")]
    [SerializeField] private float moveSpeed;
    [Header("回転速度")]
    [SerializeField] private float rotateSpeed;
    [Header("敵が攻撃する距離")]
    [SerializeField] private float attackDis;
    [Header("状態を遷移する時の目的までの距離")]
    [SerializeField] private float stateChangeDis;



    //private Vector3 currentTargetPos;
    public Vector3 cautionPos;
    private Vector3 startEnemyPos;
    private Quaternion startEnemyRot;

    [Header("反応して行動するまでの時間")]
    public int cationSearchMiliSec { get; private set; } = 1000;

    [Header("引き返すまでの時間")]
    public int returnWaitMiliSec { get; private set; } = 1000;

    [SerializeField] private Animator _enemyAnimator;



    private int waitTime;

    //private float cautionMoveDis;

    private bool isDelayed = false;

    public MovingState _currentMoveState;
    private MovingState nextMoveState;

    public ChaseOpponent _currentopponent;
    private bool _isMoving = true;
    private Vector3 _prevPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _prevPos = selfObj.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        MoveManager();
        MoveController();
    }

    private void MoveManager()
    {
        switch (_currentMoveState)
        {
            case MovingState.Wait:
                if (!isDelayed)
                {
                    isDelayed = true;
                    UniTask.Delay(waitTime).ContinueWith(() =>
                    {
                        _currentMoveState = nextMoveState;
                        isDelayed = false;
                    }
                    ).Forget();
                }

                break;
            case MovingState.Chase:
                if (Vector3.Distance(selfObj.transform.position, cautionPos) < stateChangeDis)
                {
                    _currentopponent = ChaseOpponent.None;
                    nextMoveState = MovingState.Return;
                    waitTime = returnWaitMiliSec;
                    _currentMoveState = MovingState.Wait;
                }
                //UniTask.WaitUntil(() => Vector3.Distance(transform.position, cautionPos) < stateChangeDis).ContinueWith(() =>
                //{
                //    nextMoveState = MovingState.Return;
                //    waitTime = returnWaitMiliSec;
                //    _currentMoveState = MovingState.Wait;
                //}
                // ).Forget();

                break;
            case MovingState.Return:
                if (Vector3.Distance(selfObj.transform.position, startEnemyPos) < stateChangeDis)
                {
                    splineAnimate.enabled = true;
                    _currentMoveState = MovingState.Patrol;
                }
                //UniTask.WaitUntil(() => Vector3.Distance(transform.position, startEnemyPos) < stateChangeDis).ContinueWith(() =>
                //{
                //    splineAnimate.enabled = true;
                //    _currentMoveState = MovingState.Patrol;
                //}).Forget();
                break;

        }
    }

    private void MoveController()
    {
        _isMoving = false;
        switch (_currentMoveState)
        {
            case MovingState.Wait:
                if (nextMoveState.Equals(MovingState.Chase))
                {
                    WaitMoving(cautionPos);
                }
                else
                {
                    WaitMoving(startEnemyPos);
                }
                break;
            case MovingState.Chase:
                MoveForTarget(cautionPos);
                break;
            case MovingState.Patrol:
                MoveOnSpline();
                break;
            case MovingState.Return:
                MoveForTarget(startEnemyPos);
                break;

        }
        _enemyAnimator.SetBool("run", _isMoving);
    }

    /*private void MoveForTarget(Vector3 targetPos)
    {
        Vector3 planeSelf = Vector3.ProjectOnPlane(selfObj.transform.forward, selfObj.transform.up);
        Vector3 planeTarget = Vector3.ProjectOnPlane(targetPos, selfObj.transform.up);

        float distance = Vector3.Distance(planeSelf, planeTarget);


        // ターゲットへの方向を計算
        Vector3 direction = (planeTarget - planeSelf).normalized;

        if (direction != Vector3.zero)
        {
            // 現在の向きからターゲットの向きへ少しずつ回転
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            selfObj.transform.rotation = Quaternion.Slerp(selfObj.transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }

        if (distance > attackDis)
        {
            selfObj.transform.position = Vector3.MoveTowards(selfObj.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }*/

    private void MoveForTarget(Vector3 targetPos)
    {
        // 正しい方向計算（位置ベース）
        Vector3 direction = targetPos - selfObj.transform.position;
        direction.y = 0; // 水平のみ

        float distance = direction.magnitude;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            selfObj.transform.rotation = lookRotation; // ← 即時回転
        }

        if (distance > attackDis)
        {
            selfObj.transform.position = Vector3.MoveTowards(
                selfObj.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            _isMoving = true;
        }
    }
    /*private void WaitMoving(Vector3 targetPos)
    {

        Vector3 planeSelf = Vector3.ProjectOnPlane(selfObj.transform.forward, selfObj.transform.up);
        Vector3 planeTarget = Vector3.ProjectOnPlane(targetPos, selfObj.transform.up);

        // ターゲットへの方向を計算
        Vector3 direction = (planeTarget - planeSelf).normalized;

        if (direction != Vector3.zero)
        {
            // 現在の向きからターゲットの向きへ少しずつ回転
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            selfObj.transform.rotation = Quaternion.Slerp(selfObj.transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
    }*/
    private void WaitMoving(Vector3 targetPos)
    {
        Vector3 direction = targetPos - selfObj.transform.position;
        direction.y = 0; // 水平のみ

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            selfObj.transform.rotation = lookRotation; // ← 即時回転
        }
    }

    private void MoveOnSpline()
    {
        float moved = Vector3.Distance(selfObj.transform.position, _prevPos);

        if (moved > 0.001f)
        {
            _isMoving = true;
        }

        _prevPos = selfObj.transform.position;
    }

    public void OnFind()    //playerが視界に入った時に実行する関数
    {
        if (!_currentMoveState.Equals(MovingState.Chase) && !_currentMoveState.Equals(MovingState.Wait))
        {
            cationSearchMiliSec = 100;
            _currentopponent = ChaseOpponent.Player;
            waitTime = cationSearchMiliSec;
            if (splineAnimate.enabled)
            {
                splineAnimate.enabled = false;
                startEnemyPos = selfObj.transform.position;
                startEnemyRot = selfObj.transform.rotation;

            }
            nextMoveState = MovingState.Chase;
            _currentMoveState = MovingState.Wait;
        }
    }

    public void OnCaution()    //石の効果範囲に入った時に実行する関数
    {
        if (!_currentMoveState.Equals(MovingState.Chase) && !_currentopponent.Equals(ChaseOpponent.Player))
        {
            cationSearchMiliSec = 1500;
            _currentopponent = ChaseOpponent.Stone;
            waitTime = cationSearchMiliSec;
            splineAnimate.enabled = false;
            nextMoveState = MovingState.Chase;
            startEnemyPos = selfObj.transform.position;
            startEnemyRot = selfObj.transform.rotation;
            _currentMoveState = MovingState.Wait;

        }
    }

    public void OnLose()    //playerを見失ったときに実行する関数
    {
        if (_currentMoveState.Equals(MovingState.Chase))
        {
            nextMoveState = MovingState.Return;
            waitTime = returnWaitMiliSec;
            _currentMoveState = MovingState.Wait;
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

public enum ChaseOpponent
{
    None,
    Player,
    Stone
}
