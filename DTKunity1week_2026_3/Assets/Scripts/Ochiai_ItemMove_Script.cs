using Unity.VisualScripting;
using UnityEngine;

public class Ochiai_ItemMove_Script : MonoBehaviour
{
    [Header("射出時の角度")]
    [SerializeField] private float shootAngle;
    private GameObject itemObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //アイテムの運動を制御する関数
    //アイテムのprefabにはrigidBodyを付ける前提
    public void ItemMove(Transform targetTrans, Transform startTrans)
    {
        Rigidbody itemRb = gameObject.GetComponent<Rigidbody>();
        itemRb.linearVelocity = CalculateVelocity(targetTrans.position, startTrans.position, shootAngle);
    }

    //アイテムの運動のベクトルを計算する関数
    Vector3 CalculateVelocity(Vector3 targetPos, Vector3 startPos, float angle)
    {
        // 1. 距離と方向の計算
        Vector3 diff = targetPos - startPos;
        float heightDiff = diff.y;
        Vector3 horizontalDiff = new Vector3(diff.x, 0, diff.z);
        float distance = horizontalDiff.magnitude;



        // 2. 角度をラジアンに変換
        float angleRad = angle * Mathf.Deg2Rad;

        // 3. 物理公式による初速計算
        float g = Physics.gravity.magnitude;

        // 公式: v0 = sqrt( (g * d^2) / (2 * cos^2(a) * (d * tan(a) - h)) )
        float cosA = Mathf.Cos(angleRad);
        float tanA = Mathf.Tan(angleRad);

        float denominator = 2 * cosA * cosA * (distance * tanA - heightDiff);

        // 分母が0以下（物理的にその角度で届かない）場合は発射不可
        if (denominator <= 0)
        {
            Debug.LogWarning("現在の角度ではターゲットに届きません。角度を上げてください。");
        }

        float v0 = Mathf.Sqrt((g * distance * distance) / denominator);

        // 4. 速度ベクトルの生成
        Vector3 velocity = horizontalDiff.normalized * (v0 * cosA);
        velocity.y = v0 * Mathf.Sin(angleRad);

        return velocity;
    }
}
