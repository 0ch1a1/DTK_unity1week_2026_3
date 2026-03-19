using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed;
    [Header("プレイヤーのRigidbody")]
    [SerializeField] private Rigidbody _playerRb;

    [Header("プレイヤーのHP")]
    [SerializeField] private int _playerHp;

    [Header("攻撃判定用オブジェクト")]
    [SerializeField]private GameObject _attackObj;
    [Header("カメラオブジェクト")]
    [SerializeField]private GameObject _cameraObj;

    //WASD入力を受け取る
    private Vector2 _moveInput;

    //カメラに渡す用
    private Vector2 _mouseInput;
    public Vector2 MouseInput=> _mouseInput;

    private void Start()
    {

    }
    private void Update()
    {
        Vector3 cameraForwardDir = (transform.position-_cameraObj.transform.position).normalized;
        Vector3 cameraRightDir=Vector3.Cross(Vector3.up,cameraForwardDir).normalized;
        Vector3 moveDir=cameraForwardDir*_moveInput.y+cameraRightDir*_moveInput.x;
        moveDir.y=0;
        _playerRb.linearVelocity =moveDir*_moveSpeed;
    }
    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnAttack()
    {
        Debug.Log("Attack");
        _attackObj.SetActive(true);
    }

    //攻撃判定用オブジェクトから呼ぶ
    public void Backstab()
    {
        Debug.Log("Backstab");
    }

    private void OnLook(InputValue value)
    {
        _mouseInput=value.Get<Vector2>();
    }
}