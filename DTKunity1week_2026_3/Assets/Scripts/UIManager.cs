using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemIcons;

    public void ChangeItemIcon(HangingItems hangingItems)
    {
        switch (hangingItems)
        {
            case HangingItems.None:
                for (int i = 0; i < _itemIcons.Length; i++)
                {
                    _itemIcons[i].SetActive(false);
                }
                break;
            case HangingItems.Stone:
                _itemIcons[0].SetActive(true);
                _itemIcons[1].SetActive(false);
                break;
            case HangingItems.Smoke:
                _itemIcons[0].SetActive(false);
                _itemIcons[1].SetActive(true);
                break;
        }
    }
}
