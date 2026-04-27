using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _UIObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TransitionResult()
    {
        _UIObj[0].SetActive(false);
        _UIObj[1].SetActive(true);
        Time.timeScale = 0;

    }

    public void TransitionGameOver()
    {
        _UIObj[0].SetActive(false);
        _UIObj[2].SetActive(true);
        Time.timeScale = 0;
    }
}
