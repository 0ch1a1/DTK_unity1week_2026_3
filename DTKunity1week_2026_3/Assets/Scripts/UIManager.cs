using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _UIObj;
    [SerializeField]private ResultManager _resultManager;
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
        _resultManager.SetResultScore(ScoreManager.score);

    }

    public void TransitionGameOver()
    {
        _UIObj[0].SetActive(false);
        _UIObj[2].SetActive(true);
    }
}
