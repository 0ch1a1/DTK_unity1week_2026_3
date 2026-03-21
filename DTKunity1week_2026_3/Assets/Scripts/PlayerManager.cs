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

    //WASD入力を受け取る
    private Vector2 _moveInput;

    //カメラに渡す用
    private Vector2 _mouseInput;
    public Vector2 MouseInput => _mouseInput;

    private bool _attacking = false;

    private void Start()
    {

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
        _playerRb.linearVelocity = moveDir * _moveSpeed;
        Debug.Log("moveDir" + moveDir);
        //プレイヤーの向き
        if (moveDir!=Vector3.zero)
        {
            _playersObj.transform.forward = moveDir;
        }
    }
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private async UniTask OnAttack()
    {
        if (!_attacking)
        {
            _attacking = true;
            Debug.Log("Attack");
            _attackObj.SetActive(true);
            await UniTask.Delay(_attackingSpan);
            _attackObj.SetActive(false);
        }
        await UniTask.Delay(_attackCoolDown).ContinueWith(() => _attacking = false);
    }

    //攻撃判定用オブジェクトから呼ぶ
    public async UniTask Backstab()
    {
        Debug.Log("Backstab");
        await HitStopManager.DoHitStop(0.2f,800);
    }

    private void OnLook(InputValue value)
    {
        _mouseInput = value.Get<Vector2>();
    }
}