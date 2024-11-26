using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource engineSound; // Motor sesi
    public AudioSource roadSound;   // Çarpma sesi
    public AudioSource cakilSound;     // Korna sesi

   
    public void PlayStartSound()
    {
        engineSound.Play();
    }


    public void PlayRoadSound()
    {
        roadSound.Play();
    }

    public void PlayCakilSound()
    {
        cakilSound.Play();
    }
}
