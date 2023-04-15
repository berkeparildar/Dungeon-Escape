using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public Image selectionImage;
    public static UIManager Instance => _instance;

    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
    }

    public TMP_Text playerGemCount;
    public TMP_Text uiGemCount;
    
    public void OpenShop(string gemCount)
    {
        playerGemCount.text = gemCount + " " + "G";
    }
                    
    public void UpdateSelection(int index)
    {
        switch (index)
        {
            case 0:
                selectionImage.rectTransform.anchoredPosition = new Vector2(-123.9f, 4);
                break;
            case 1: selectionImage.rectTransform.anchoredPosition = new Vector2(-123.9f, -106);
                break;
            case 2: selectionImage.rectTransform.anchoredPosition = new Vector2(-123.9f, -216);
                break;
        }
    }

    public void UpdateUIGemCount(int gem)
    {
        uiGemCount.text = gem.ToString();
    }
}
