using Cysharp.Threading.Tasks;
using UnityEngine;

public class Ochiai_FieldEffect_Script : MonoBehaviour
{
    [SerializeField] private  FieldType currentType;
    [SerializeField] private float DestroyTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FieldDestroy();
    }

    private void FieldDestroy()
    { 
        DestroyTime -= Time.deltaTime;
        if(DestroyTime < 0)
        {
            Destroy(gameObject);
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (currentType == FieldType.Search)
        {
            if (other.CompareTag("Back"))
            {
                GameObject enemyObj = other.gameObject;
                Ochiai_EnemyMove_Script enemyMove_Script = enemyObj.GetComponent<Ochiai_EnemyMove_Script>();
                enemyMove_Script.cautionPos = transform.position;
                enemyMove_Script.OnCaution();
            }
        }
    }

}
public enum FieldType
{
    None,
    Search,
    invisible
}
