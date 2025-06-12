using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the button
        audioSource = GetComponent<AudioSource>();

        // Get the Button component and add a listener for the click event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        // Play the sound when the button is clicked
        audioSource.Play();
    }
}