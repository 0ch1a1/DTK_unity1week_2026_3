using Unity.VisualScripting;
using UnityEngine;

public class AttackManagar : MonoBehaviour
{
    [Header("親オブジェクト")]
    [SerializeField] private PlayerManager _parentPM;

    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        _parentPM.Backstab();
    }
}
