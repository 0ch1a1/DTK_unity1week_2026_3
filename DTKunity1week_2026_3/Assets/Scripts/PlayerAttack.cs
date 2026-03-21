using UnityEngine;

public class PlayerAttack: MonoBehaviour
{
    // すでに当たったかどうかを管理
    public bool hasHit = false;

    // 攻撃が終わるタイミング（または生成時）にリセットする用
    public void ResetHit()
    {
        hasHit = false;
    }
}