using UnityEngine;
using UnityEngine.SceneManagement;

public class OnBack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Start"); // Burada önceki sahne adını girin
    }
}
