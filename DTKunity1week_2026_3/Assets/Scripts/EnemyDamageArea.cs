using UnityEngine;

public class EnemyDamageArea : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    [SerializeField] private GameObject hitEffectPrefab;    // 前面（ガード用火花）
    [SerializeField] private GameObject damageEffectPrefab; // 背後（ダメージ用血飛沫）

    void OnTriggerEnter(Collider other)
    {
        // 1. タグチェック
        if (!other.CompareTag("PlayerAttack")) return;

        // 2. 多段ヒット防止チェック
        PlayerAttack attack = other.GetComponent<PlayerAttack>();
        if (attack != null && attack.hasHit) return;

        // --- ここから判定ロジックの実装 ---

        // 3. 攻撃が来た方向を計算
        Vector3 attackDirection = (other.transform.position - transform.position).normalized;
        
        // 4. 内積で前後判定（transform.forward を基準にする）
        float dot = Vector3.Dot(transform.forward, attackDirection);

        Vector3 hitPos = other.ClosestPoint(transform.position);

        if (dot < 0) // 正面判定（今回は背後をガードに設定）
        {
            Debug.Log("正面ガード成功！");
            PlayEffect(hitEffectPrefab, hitPos);
        }
        else // 背面判定（ダメージ）
        {
            hp -= 100;
            Debug.Log("背後ヒット！ 残りHP: " + hp);
            PlayEffect(damageEffectPrefab, hitPos);

            if (hp <= 0) Die();
        }

        // 5. 攻撃済みのフラグを立てる
        if (attack != null)
        {
            attack.hasHit = true; 
        }
    }

    void PlayEffect(GameObject prefab, Vector3 pos)
    {
        if (prefab != null) Instantiate(prefab, pos, Quaternion.identity);
    }

    void Die()
    {
        Debug.Log("エネミー撃破");
        Destroy(gameObject);
    }
}