using UnityEngine;

public class HitSounds : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;

    public void AssignNewSoundClip(AudioClip audioClip)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }
}
