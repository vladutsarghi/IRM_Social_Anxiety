using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PlaySoundAndAnimate : MonoBehaviour
{
    private AudioSource audioSource;
    public Animator baristaAnimator; // Reference to the Animator of the other character
    private bool hasPlayed = false;

    void Start()
    {
        // Get the AudioSource component on this object
        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerAnimationAndSound()
    {
        // Trigger animation and play sound only once
        if (!hasPlayed)
        {
            if (audioSource != null)
            {
                audioSource.Play(); // Play the sound
            }

            if (baristaAnimator != null)
            {
                baristaAnimator.SetBool("start_talk", true); // Set the Boolean parameter on the other character
            }

            hasPlayed = true; // Mark as played
            StartCoroutine(ResetAnimationParameter());
        }
    }

    private IEnumerator ResetAnimationParameter()
    {
        if (audioSource != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length); // Wait for the audio to finish
        }

        if (baristaAnimator != null)
        {
            baristaAnimator.SetBool("start_talk", false); // Reset the Boolean parameter
        }
    }
}
