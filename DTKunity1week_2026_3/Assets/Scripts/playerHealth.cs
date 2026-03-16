using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI操作に必要

public class playerHealth : MonoBehaviour
{
    // プレイヤーの最大HPと現在のHP
    public int maxHp = 1;
    private int currentHp;

    // スライダーの参照
    public Slider hpSlider;

    void Start()
    {
        // 初期設定
        currentHp = maxHp; // HPを最大値に設定
        hpSlider.maxValue = maxHp; // スライダーの最大値を設定
        hpSlider.value = currentHp; // 現在のHPを反映
    }

    public void TakeDamage(int damage)
    {
        // HPを減らす処理
        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // スライダーに現在のHPを反映
        hpSlider.value = currentHp;

        // HPが0になったときの処理
        if (currentHp == 0)
        {
            Debug.Log("ゲームオーバー！");
            // ここにゲームオーバーの処理を追加
        }
    }

    void OnTriggerEnter(Collider other)
{
    if (other.gameObject.tag == "Enemy") // 敵と接触した場合
    {
        TakeDamage(1); // HPを1減らす
    }
}
}