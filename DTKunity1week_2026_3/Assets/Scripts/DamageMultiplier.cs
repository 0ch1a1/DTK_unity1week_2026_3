using UnityEngine;

// 部位ごとのダメージ倍率を設定するクラス
public class DamageMultiplier : MonoBehaviour
{
    [SerializeField] private float damageMultiplier = 1.0f; // 部位ごとの倍率
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        // ルートオブジェクトから EnemyHealth を取得
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    // 弾や攻撃が当たった時の処理
    private void OnCollisionEnter(Collision collision)
    {
        // 攻撃側が DamageSender を持っている場合のみ処理
        DamageSender sender = collision.gameObject.GetComponent<DamageSender>();
        if (sender != null && enemyHealth != null)
        {
            float finalDamage = sender.damage * damageMultiplier;
            enemyHealth.TakeDamage(finalDamage);
        }
    }
}