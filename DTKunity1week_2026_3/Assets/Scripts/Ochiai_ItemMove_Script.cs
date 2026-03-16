using UnityEngine;

public class Ochiai_ItemMove_Script : MonoBehaviour
{
    [Header("射出時の角度")]
    [SerializeField] private float moveAngle;
    [Header("プレイヤーの位置 (Don't Set)")]
    public Transform spawnTrans;
    [Header("マーカの位置(Don't Set)")]
    public Transform markerTrans;
    [Header("このオブジェクト")]
    [SerializeField] private GameObject itemObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ItemMove(markerTrans, spawnTrans, itemObj, moveAngle);
    }

    //アイテムの運動を制御する関数
    //アイテムのprefabにはrigidBodyを付ける前提
    public void ItemMove(Transform targetTrans,Transform startTrans, GameObject itemObj, float angle)
    {
        Rigidbody itemRb = itemObj.GetComponent<Rigidbody>();
        itemRb.linearVelocity = CalculateVelocity(targetTrans.position, startTrans.position, angle);
    }

    //アイテムの運動のベクトルを計算する関数
    Vector3 CalculateVelocity(Vector3 targetPos, Vector3 startPos, float angle)
    {
        Vector3 direction = targetPos - startPos; // 目標までの方向ベクトル
        float heightDiff = direction.y;          // 高さの差
        direction.y = 0;                         // 水平方向の距離計算のためyを0に
        float distance = direction.magnitude;    // 水平距離

        float angleRad = angle * Mathf.Deg2Rad;  // 角度をラジアンに変換
        direction.y = distance * Mathf.Tan(angleRad); // 角度に合わせたベクトル高さ
        distance += heightDiff / Mathf.Tan(angleRad); // 高低差の補正

        // 放物線の初速計算式: v = sqrt(g * d^2 / (2 * cos^2(a) * (d * tan(a) - h)))
        float gravity = Physics.gravity.magnitude;
        float v0 = Mathf.Sqrt(gravity * distance * distance / (2 * Mathf.Pow(Mathf.Cos(angleRad), 2) * (distance * Mathf.Tan(angleRad) - heightDiff)));

        return direction.normalized * v0;
    }
}
