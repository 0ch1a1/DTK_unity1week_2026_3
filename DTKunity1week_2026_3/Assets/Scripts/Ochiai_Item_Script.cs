using UnityEngine;

public class Ochiai_Item_Script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemMove(Transform targetTrans, GameObject itemObj, float angle)
    {
        Rigidbody itemRb = itemObj.GetComponent<Rigidbody>();
        itemRb.linearVelocity = CalculateVelocity(targetTrans.position, itemObj.transform.position, angle);
    }

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
