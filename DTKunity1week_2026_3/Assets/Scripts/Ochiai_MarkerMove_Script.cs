using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;


public class Ochiai_MarkerMove_Script : MonoBehaviour
{
    [SerializeField] private Ochiai_ItemSpawn_Script itemSpawn_Script;
    [SerializeField] private GameObject _markerObj;
    [SerializeField] private GameObject _playerObj;
    [Header("ƒvƒŒƒCƒ„[‚ÌAnimator")]
    [SerializeField] private Animator _playerAnimator;

    private Vector3 currentPos;

    private bool isHolding;

    [SerializeField]private PlayerManager _pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _markerObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //UsingItem();
        if (isHolding)
        {
            _markerObj.transform.position = MarkerMove();
        }
    }

    //ƒ}[ƒJ[‚ð“®‚©‚·ŠÖ”
    private Vector3 MarkerMove()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward
        );
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.tag == "ground")
            {
                currentPos = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
            }
            else
            {
                //currentPos = new Vector3(playerTrans.position.x, groundTrans.position.y + 0.01f, playerTrans.position.z);
            }
            return currentPos;
        }


        return currentPos;
    }

    private void MarkerVisializeChange(bool flag)
    {
        _markerObj.SetActive(flag);
    }
    private async UniTask OnItem(InputValue value)
    {
        if (_pm.controllerStop == true)
        {
            return;
        }
        else
        {
            if (value.isPressed)
            {
                if (!isHolding)
                {
                    // ‰Ÿ‚µ‚½uŠÔ
                    MarkerVisializeChange(true);
                }

                isHolding = true;
            }
            else
            {
                // —£‚µ‚½uŠÔ
                //“™‰¿•ûŒü‚ÖƒvƒŒƒCƒ„[ƒ‚ƒfƒ‹‚ðŒü‚¯‚é
                _pm.controllerStop = true;
                _playerObj.transform.forward = _markerObj.transform.position - _playerObj.transform.position;
                _playerAnimator.SetBool("item", true);
                itemSpawn_Script.ItemSpawn();
                MarkerVisializeChange(false);
                await UniTask.WaitUntil(() =>
                    {
                        var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);
                        return state.IsName("Armature|Throw") && state.normalizedTime >= 1.0f;
                    });
                _playerAnimator.SetBool("item", false);
                isHolding = false;
                _pm.controllerStop=false;
            }
        }
    }
}
