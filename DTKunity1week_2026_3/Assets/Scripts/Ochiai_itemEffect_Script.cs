using UnityEngine;

public class Ochiai_itemEffect_Script : MonoBehaviour
{
    [Header("このオブジェクトのアイテムの種類")]
    [SerializeField] private HangingItems thisItem; 
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
    }

    //石が着弾したときの処理の関数
    private void StoneEffect()
    {

    }

    //けむり玉が着弾したときの処理の関数
    private void SmokeEffect()
    {

    }
}
