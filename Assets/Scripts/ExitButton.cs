using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Oyun editörde çalışıyorsa durdurur
        #else
            Application.Quit(); // Build edilen uygulamada oyunu kapatır
        #endif
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Start"); // Ana menü sahnesine dönmek için
    }
}
