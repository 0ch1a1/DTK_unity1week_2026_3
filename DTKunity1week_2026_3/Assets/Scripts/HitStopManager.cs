using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    private static bool _isStopping = false;

    public static async UniTask DoHitStop(float timeScale, int stopTime)
    {
        if (_isStopping) return;

        _isStopping = true;

        Time.timeScale = 0;
        await UniTask.Delay(stopTime, ignoreTimeScale: true).ContinueWith(() => Time.timeScale = 1f);

        _isStopping = false;
    }
}