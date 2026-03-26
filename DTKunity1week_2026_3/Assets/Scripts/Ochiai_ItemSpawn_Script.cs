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
    [SerializeField] private Ochiai_MarkerMove_Script markerMove_Script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //持っている（生成する）アイテムの種類を変える関数
    public void ChangeSpawnItem(HangingItems getItem)
    {
        currentItem = getItem;

        switch (currentItem)
        {
            case HangingItems.None:
                currentSpawnObj = null; 
                break;
            case HangingItems.Stone:
                currentSpawnObj = spawnObjs[0];
                break;
            case HangingItems.Smoke: 
                currentSpawnObj = spawnObjs[1];
                break;
        }
    }

    //アイテムを生成する関数
    //アイテムを使用するときにこの関数を処理する
    public void ItemSpawn()
    {
        if(currentItem != HangingItems.None)
        {
            GameObject cloneItemObj = Instantiate(currentSpawnObj, spawnTrans.position, spawnTrans.rotation);
            Ochiai_ItemMove_Script ItemMove_Script = cloneItemObj.GetComponent<Ochiai_ItemMove_Script>();
            ItemMove_Script.markerTrans = markerMove_Script.transform;
            ItemMove_Script.spawnTrans = spawnTrans;
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
