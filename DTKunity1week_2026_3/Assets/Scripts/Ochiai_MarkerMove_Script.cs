using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ochiai_MarkerMove_Script : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform groundTrans;
    [SerializeField] private Ochiai_ItemSpawn_Script itemSpawn_Script;
    public Ochiai_ItemMove_Script itemMove_Script;
    private bool markerVisiableFlag;
    private Vector3 currentPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        markerVisiableFlag = false;
        GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UsingItem();
    }

    //マーカーを動かす関数
    private Vector3 MarkerMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "ground")
            {
                currentPos = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
            }
            else
            {
                //currentPos = new Vector3(playerTrans.position.x, groundTrans.position.y + 0.01f, playerTrans.position.z);
            }
        }
        

        return currentPos;
    }

    private void MarkerVisializeChange(bool flag)
    {
        markerVisiableFlag = flag;
        GetComponent<Renderer>().enabled = flag;
    }

    public void UsingItem()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            MarkerVisializeChange(true);
            //debug
            itemSpawn_Script.ChangeSpawnItem(HangingItems.Stone);
        }
        if (Mouse.current.rightButton.isPressed)
        {
            transform.position = MarkerMove();
            
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            itemSpawn_Script.ItemSpawn();
            MarkerVisializeChange(false);
        }
    }
}
