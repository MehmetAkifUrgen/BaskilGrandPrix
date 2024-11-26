using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceResultsDisplay : MonoBehaviour
{
    public Text resultsText; // UI Text nesnesini burada atayın

    private void Start()
    {
        DisplayResults();
    }

    private void DisplayResults()
    {
        // Yarış sıralamasını göster
        resultsText.text = "Yarış Sıralaması:\n";

        foreach (string result in RaceManager.playerFinishOrder)
        {
            resultsText.text += result + "\n";
        }
    }
 
}
