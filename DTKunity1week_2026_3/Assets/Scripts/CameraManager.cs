using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("カメラとプレイヤーの距離")]
    [SerializeField] private float _dis;

    [Header("カメラ高さ上限")]
    [SerializeField] private float _limHigh;
    [Header("カメラ低さ上限")]
    [SerializeField]private float _limLow;
    
    [Header("カメラ移動速度")]
    [SerializeField]private float _smoothSpeed;

    [Header("プレイヤーオブジェクト")]
    [SerializeField]private PlayerManager _pm;

    [Header("CameraOrigin")]
    [SerializeField] private GameObject _COObj;

    //マウスの入力を受け取る
    private Vector2 _mouseInput;

    private void LateUpdate()
    {
        _mouseInput = _pm.MouseInput;
        
        //横回転
        Debug.Log("mouse" + _mouseInput);
        if (Mathf.Abs(_mouseInput.x) > 0.0000001f)
        {
            _mouseInput.x *= _smoothSpeed;
            transform.RotateAround(_COObj.transform.position, _COObj.transform.up, _mouseInput.x);
        }
        else
        {
            _mouseInput.x = 0;
        }
        //縦回転
        if (Mathf.Abs(_mouseInput.y) > 0.0000001f)
        {
            //縦回転制限
            if (transform.position.y > _COObj.transform.position.y + _limHigh)
            {
                Vector3 limitPos = new Vector3(transform.position.x, _COObj.transform.position.y + _limHigh, transform.position.z);
                transform.position = limitPos;
            }
            else if (transform.position.y < _COObj.transform.position.y - _limLow)
            {
                Vector3 limitPos = new Vector3(transform.position.x, _COObj.transform.position.y - _limLow, transform.position.z);
                transform.position = limitPos;
            }
            else
            {
                _mouseInput.y *= _smoothSpeed;
                transform.RotateAround(_COObj.transform.position, transform.right, _mouseInput.y);
            }
        }
        else
        {
            _mouseInput.y = 0;
        }

        //カメラの向きと距離調整
        Vector3 toPlayerDir=(transform.position-_COObj.transform.position).normalized;
        transform.position=_COObj.transform.position+toPlayerDir*_dis;
        transform.LookAt(_COObj.transform);
    }
}
