using UnityEngine;

public class Ochiai_ItemTake_Script : MonoBehaviour
{
    [Header("���̃I�u�W�F�N�g����擾����A�C�e���̎��")]
    [SerializeField] private HangingItems thisItem;
    [Header("�A�C�e���𐶐����邽�߂̃X�N���v�g")]
    [SerializeField] private Ochiai_ItemSpawn_Script itemSpawn_Script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if(other.gameObject.tag == "")
        {
            TakeItem();
        }
    }
}
