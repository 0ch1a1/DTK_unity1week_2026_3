using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    private static TextMeshProUGUI _scoreTMP;
    [SerializeField]private TextMeshProUGUI _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scoreTMP=_text;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void SetScoreTMP(int addScore)
    {
        score+=addScore;
        _scoreTMP.text = score.ToString();
    }
}
