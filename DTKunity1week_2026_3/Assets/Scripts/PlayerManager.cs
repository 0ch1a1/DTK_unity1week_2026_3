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

    private Vector2 _moveInput;

    private void Start()
    {

    }
    private void Update()
    {
        Vector3 _move = new Vector3(_moveInput.x, 0, _moveInput.y);
        _playerRb.linearVelocity = _move * _moveSpeed;
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
}