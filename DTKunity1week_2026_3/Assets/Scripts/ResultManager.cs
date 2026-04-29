using UnityEngine;
using TMPro;
public class ResultManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _scoreText;

    public void SetResultScore(int score)
    {
        _scoreText.text="Score : "+score.ToString();
    }
}
