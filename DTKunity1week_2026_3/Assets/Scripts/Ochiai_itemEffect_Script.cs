using UnityEngine;
using UnityEngine.InputSystem;

public class Ochiai_ItemEffect_Script : MonoBehaviour
{
    [Header("このオブジェクトのアイテムの種類")]
    [SerializeField] private HangingItems thisItem;
    [SerializeField] private GameObject field;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //アイテムが着弾したときの処理の関数
    public void LandingEffect()
    {
        switch (thisItem)
        {
            case HangingItems.Stone:
                StoneEffect();
                break;
            case HangingItems.Smoke:
                SmokeEffect();
                break;
        }
        Destroy(gameObject);
    }

    //石が着弾したときに実行する関数
    private void StoneEffect()
    {
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Ray ray = new Ray(rayPos, -transform.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "ground")
            {
                Vector3 genePos = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                GameObject searchObj  = Instantiate(field, genePos, field.transform.rotation);
            }
        }
    }

    //けむり玉が着弾したときに実行する関数
    private void SmokeEffect()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "ground")
            {
                Vector3 genePos = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                GameObject searchObj = Instantiate(field, genePos, field.transform.rotation);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LandingEffect();
    }
}
