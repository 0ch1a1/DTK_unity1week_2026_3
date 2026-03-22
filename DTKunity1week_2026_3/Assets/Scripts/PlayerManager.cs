using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;


public class PlayerManager : MonoBehaviour
{
    [Header("【プレイヤーステータスパラメータ】")]
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed;


    [Header("プレイヤーのHP")]
    [SerializeField] private int _playerHp;

    [Header("攻撃のリキャスト[ms]")]
    [SerializeField] private int _attackCoolDown;

    [Header("攻撃判定持続時間[ms]")]
    [SerializeField] private int _attackingSpan;

    [Header("【アタッチフィールド】")]
    [Header("プレイヤーのRigidbody")]
    [SerializeField] private Rigidbody _playerRb;

    [Header("攻撃判定用オブジェクト")]
    [SerializeField] private GameObject _attackObj;
    [Header("カメラオブジェクト")]
    [SerializeField] private GameObject _cameraObj;

    [Header("プレイヤーズオブジェクト")]
    [SerializeField] private GameObject _playersObj;
    [Header("プレイヤーのAnimator")]
    [SerializeField] private Animator _playerAnimator;


    //WASD入力を受け取る
    private Vector2 _moveInput;

    //カメラに渡す用
    private Vector2 _mouseInput;
    public Vector2 MouseInput => _mouseInput;

    private bool _attacking = false;
    //攻撃中は静止
    public bool controllerStop = false;
    //アニメーション終了を取得
    private AnimatorStateInfo _animeInfo;

    private void Start()
    {
        _playerRb.linearVelocity = Vector3.zero;
    }
    private void Update()
    {
        //カメラ正面、直交右方向ベクトル計算
        Vector3 cameraForwardDir = transform.position - _cameraObj.transform.position;
        cameraForwardDir.y = 0;
        cameraForwardDir.Normalize();
        Vector3 cameraRightDir = Vector3.Cross(Vector3.up, cameraForwardDir);
        cameraRightDir.y = 0;
        cameraRightDir.Normalize();
        //移動方向ベクトル計算・代入
        Vector3 moveDir = cameraForwardDir * _moveInput.y + cameraRightDir * _moveInput.x;
        moveDir.y = 0;
        //速度ベクトル代入
        Vector3 currentVelocity = _playerRb.linearVelocity;
        if (controllerStop == true)
        {
            _playerRb.linearVelocity = Vector3.zero;
        }
        else
        {
            _playerRb.linearVelocity = new Vector3(moveDir.x * _moveSpeed, currentVelocity.y, moveDir.z * _moveSpeed) * Time.deltaTime * 100;
        }
        //プレイヤーの向き
        if (moveDir != Vector3.zero)
        {
            _playersObj.transform.forward = moveDir;
        }
        //アニメーション
        if (_moveInput.magnitude > 0 && !controllerStop)
        {
            _playerAnimator.SetBool("run", true);
        }
        else
        {
            _playerAnimator.SetBool("run", false);
        }
    }
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private async UniTask OnAttack()
    {
        if (controllerStop)
        {
            return;
        }
        else
        {
            if (!_attacking)
            {
                controllerStop = true;
                _attacking = true;
                Debug.Log("Attack");
                _playerAnimator.SetBool("attack", true);
                UniTask.Delay(200).ContinueWith(() => _attackObj.SetActive(true)).Forget();
                await UniTask.WaitUntil(() =>
                    {
                        var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);
                        return state.IsName("Armature|Attack_2") && state.normalizedTime >= 1.0f;
                    });
                _attackObj.SetActive(false);
                _playerAnimator.SetBool("attack", false);
                controllerStop = false;
            }
            await UniTask.Delay(_attackCoolDown).ContinueWith(() => _attacking = false);
        }

    }

    //攻撃判定用オブジェクトから呼ぶ
    public async UniTask Backstab()
    {
        Debug.Log("Backstab");
        await HitStopManager.DoHitStop(0.2f, 800);
    }

    private void OnLook(InputValue value)
    {
        _mouseInput = value.Get<Vector2>();
    }

    public void GameOver()
    {
        Debug.Log("死んだ");
    }
}