using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using TMPro;

public class EnemyLookArea : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float rayDistance;
    [SerializeField] private int rayCount; // 負荷軽減のため少し調整（10度刻みなら36）
    [SerializeField] private string targetTag = "Player"; // 検知対象のタグ

    [Header("アクティブにするオブジェクト")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private GameObject _center;
    [SerializeField] private Rigidbody _rb;

    [Header("敵の動きの制御するスクリプト")]
    [SerializeField] private Ochiai_EnemyMove_Script _enemyMoveScript;

    private bool attacking = false;
    private bool _search = true;
    private GameObject _foundObj;

    private void Start()
    {
        // 初期状態ではオフにしておく（必要であれば）
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
    }

    private void Update()
    {
        bool foundPlayer = false;
        _search = true;

        float spread = 30f; // 視野角（左右30度）
        int rayNum = 5;

        for (int i = 0; i < rayNum; i++)
        {
            float angle = -spread + i * (spread * 2 / (rayNum - 1));
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag(targetTag))
                {
                    foundPlayer = true;
                    _foundObj = hit.collider.gameObject;
                    _enemyMoveScript.cautionPos = hit.collider.gameObject.transform.position;
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                    break;
                }
            }

            Debug.DrawRay(transform.position, dir * rayDistance, Color.cyan);
        }
        // 検知結果に基づいてオブジェクトをアクティブ化
        if (foundPlayer && !attacking)
        {
            attacking = true;
            _enemyMoveScript.OnFind();
            ActivateTarget().Forget();
        }
    }

    private async UniTask ActivateTarget()
    {
        if (objectToActivate != null && !objectToActivate.activeSelf)
        {
            gameObject.transform.LookAt(_foundObj.transform.position);
            _enemyAnimator.SetBool("attack", true);
            await UniTask.Delay(400);
            objectToActivate.SetActive(true);
            await UniTask.WaitUntil(() =>
            {
                var state = _enemyAnimator.GetCurrentAnimatorStateInfo(0);
                return state.IsName("Armature|Attack_2") && state.normalizedTime >= 1.0f;
            });
            _enemyAnimator.SetBool("attack", false);
            objectToActivate.SetActive(false);
            attacking = false;
        }
    }
}