using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using TMPro;

public class EnemyLookArea : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private int rayCount = 36; // 負荷軽減のため少し調整（10度刻みなら36）
    [SerializeField] private string targetTag = "Player"; // 検知対象のタグ

    [Header("アクティブにするオブジェクト")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private Animator _enemyAnimator;

    private bool attacking=false;
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

        for (int i = 0; i < rayCount; i++)
        {
            // レイの角度計算（360度全方位）
            float angle = 240 - i * 360f / 360;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, rayDistance))
            {
                // プレイヤーのタグをチェック
                if (hit.collider.CompareTag(targetTag))
                {
                    foundPlayer = true;
                    // デバッグ用（検知したレイを赤くする）
                    Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
                    break; // 一人見つけたらループを抜ける
                }
            }

            // 通常のレイの可視化
            Debug.DrawRay(transform.position, dir * rayDistance, Color.cyan);
        }

        // 検知結果に基づいてオブジェクトをアクティブ化
        if (foundPlayer&&!attacking)
        {
            attacking=true;
            ActivateTarget().Forget();
        }
    }

    private async UniTask ActivateTarget()
    {
        if (objectToActivate != null && !objectToActivate.activeSelf)
        {
            _enemyAnimator.SetBool("attack",true);
            await UniTask.Delay(400);
            objectToActivate.SetActive(true);
            await UniTask.WaitUntil(() =>    
            {
                var state = _enemyAnimator.GetCurrentAnimatorStateInfo(0);
                return state.IsName("Armature|Attack_2") && state.normalizedTime >= 1.0f;
            });
            _enemyAnimator.SetBool("attack",false);
            objectToActivate.SetActive(false);
            attacking=false;
        }
    }
}