using UnityEngine;

public class EnemyLookArea : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private int rayCount = 36; // 負荷軽減のため少し調整（10度刻みなら36）
    [SerializeField] private string targetTag = "Player"; // 検知対象のタグ

    [Header("アクティブにするオブジェクト")]
    [SerializeField] private GameObject objectToActivate;

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
        if (foundPlayer)
        {
            ActivateTarget();
        }
        else
        {
            Debug.Log("Non");
        }
    }

    private void ActivateTarget()
    {
        if (objectToActivate != null && !objectToActivate.activeSelf)
        {
            objectToActivate.SetActive(true);
            Debug.Log("プレイヤーを検知！オブジェクトを起動しました。");
        }
    }
}