using UnityEngine;

public class Ochiai_ItemTake_Script : MonoBehaviour
{
    [Header("このオブジェクトから取得するアイテムの種類")]
    [SerializeField] private HangingItems thisItem;
    [Header("アイテムを生成するためのスクリプト")]
    [SerializeField] private Ochiai_ItemSpawn_Script itemSpawn_Script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //アイテムを取得する関数, 各アイテム取得場所のオブジェクトにつける
    //アイテムを取得するときにこの関数を処理する
    public void TakeItem()
    {
        itemSpawn_Script.ChangeSpawnItem(thisItem);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "")
        {
            TakeItem();
        }
    }
}
