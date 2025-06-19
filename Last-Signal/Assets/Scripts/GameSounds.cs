using UnityEngine;

public class GameSounds : MonoBehaviour
{
    [SerializeField] private AudioSource shootSound; // AudioSource voor schietgeluid
    [SerializeField] private AudioSource destroySound; // AudioSource voor vernietigingsgeluid

    // Singleton-patroon
    public static GameSounds Instance { get; private set; }

    private void Awake()
    {
        // Zorg ervoor dat er maar één GameSounds is
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Behoud tussen scènes
        }
        else
        {
            Destroy(gameObject); // Vernietig duplicaten
        }
    }

    // Speel schietgeluid af
    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            shootSound.Play();
        }
    }

    // Speel vernietigingsgeluid af
    public void PlayDestroySound()
    {
        if (destroySound != null)
        {
            destroySound.Play();
        }
    }
}