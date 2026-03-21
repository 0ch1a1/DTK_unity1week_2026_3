using Cysharp.Threading.Tasks;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public GameObject _center;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_center.transform.position,Vector3.up,100*Time.deltaTime);
    }
}
