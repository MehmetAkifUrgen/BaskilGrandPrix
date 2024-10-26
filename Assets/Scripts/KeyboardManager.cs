using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public RectTransform panelRectTransform;  // Panel'in RectTransform'u (tüm inputları kapsayan panel)
    public float offset = 200f;  // Klavye açıldığında yukarı kaydırma miktarı
    private Vector2 originalPos;

    void Start()
    {
        originalPos = panelRectTransform.anchoredPosition;  // Panelin orijinal pozisyonunu kaydet
    }

    void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            // Klavye açıldığında paneli yukarı kaydır
            panelRectTransform.anchoredPosition = new Vector2(originalPos.x, originalPos.y + offset);
        }
        else
        {
            // Klavye kapandığında paneli orijinal yerine döndür
            panelRectTransform.anchoredPosition = originalPos;
        }
    }
}
