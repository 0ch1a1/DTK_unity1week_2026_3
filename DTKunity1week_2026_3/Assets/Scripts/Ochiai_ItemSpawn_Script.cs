using UnityEngine;
using UnityEngine.InputSystem;

public class Ochiai_ItemSpawn_Script : MonoBehaviour
{
    [Header("アイテム出現位置")]
    [SerializeField] private Transform spawnTrans;
    [Header("出現させるアイテム(0:石, 1:煙幕)")]
    [SerializeField] GameObject[] spawnObjs;
    private GameObject currentSpawnObj;
    [Header("持っているアイテムの種類(Don't Set)")]
    public HangingItems currentItem;
    [Header("このスクリプト")]
    [SerializeField] private Ochiai_ItemSpawn_Script thisScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpawnItem(currentItem, spawnObjs);
    }

    //持っている（生成する）アイテムの種類を変える関数
    private void ChangeSpawnItem(HangingItems hangingItem, GameObject[] itemObjs)
    {
        switch (hangingItem)
        {
            case HangingItems.None:
                currentSpawnObj = null; 
                break;
            case HangingItems.Stone:
                currentSpawnObj = itemObjs[0];
                break;
            case HangingItems.Smoke: 
                currentSpawnObj = itemObjs[1];
                break;
        }
    }

    //アイテムを生成する関数
    //アイテムを使用するときにこの関数を処理する
    public void ItemSpawn()
    {
        if(currentItem != HangingItems.None)
        {
            GameObject cloneItemObj = Instantiate(currentSpawnObj, spawnTrans);
            currentItem = HangingItems.None;
        }
    }
}

//アイテムの種類
public enum HangingItems
{
    None,
    Stone,
    Smoke
}
