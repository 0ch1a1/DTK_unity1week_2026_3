using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class Ochiai_ItemTake_Script : MonoBehaviour
{
    [Header("���̃I�u�W�F�N�g����擾����A�C�e���̎��")]
    [SerializeField] private HangingItems thisItem;
    [Header("�A�C�e���𐶐����邽�߂̃X�N���v�g")]
    [SerializeField] private Ochiai_ItemSpawn_Script itemSpawn_Script;
    [Header("アイテムObject")]
    [SerializeField] private GameObject[] _itemObjs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (thisItem)
        {
            case HangingItems.Stone:
                _itemObjs[0].SetActive(true);
                _itemObjs[1].SetActive(false);
                break;
            case HangingItems.Smoke:
                _itemObjs[0].SetActive(false);
                _itemObjs[1].SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //�A�C�e�����擾����֐�, �e�A�C�e���擾�ꏊ�̃I�u�W�F�N�g�ɂ���
    //�A�C�e�����擾����Ƃ��ɂ��̊֐�����������
    public void TakeItem()
    {
        itemSpawn_Script.ChangeSpawnItem(thisItem);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TakeItem();
            UniTask.Delay(1000).Forget();
        }
    }
}
