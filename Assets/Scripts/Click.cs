using UnityEngine;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Loading"); // "Game" sahnesine geçiş yaparak oyunu başlatın
    }

}
