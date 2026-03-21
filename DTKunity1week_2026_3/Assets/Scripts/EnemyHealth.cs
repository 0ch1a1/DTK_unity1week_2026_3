using UnityEngine;

// 敵本体の HP 管理クラス
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;
    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    // ダメージを受ける処理
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} is dead!");
        // 死亡アニメーションや破壊処理をここに追加
        Destroy(gameObject);
    }
}