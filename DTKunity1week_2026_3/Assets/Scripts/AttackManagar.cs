using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("親オブジェクト")]
    [SerializeField] private PlayerManager _parentPM;

    private bool _wasFront=false;

    private void Start()
    {
    }
    private async void OnTriggerEnter(Collider other)
    {
        _wasFront=false;
        if (other.CompareTag("Front"))
        {
            _wasFront=true;
        }
        else if (other.CompareTag("Back")&&!_wasFront)
        {
            await _parentPM.Backstab();
        }
        else if (other.CompareTag(""))
        {
            //アイテム取得
        }
        else if (other.CompareTag(""))
        {
            //他アクション
        }
    }
}
