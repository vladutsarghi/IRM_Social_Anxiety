using UnityEngine;
using System.Collections;

public class RoomEntryWave : MonoBehaviour
{
    public Animator baristaAnimator; // Reference to the Animator of the barista
    public float waveDuration = 2f; // Duration of the wave animation
    private bool hasWaved = false; // Ensure it triggers only once

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("MainCamera") && !hasWaved)
        {
            Debug.Log("Player entered the room trigger!"); // Debug message
            hasWaved = true;
            TriggerWave();
        }
    }

    private void TriggerWave()
    {
        if (baristaAnimator != null)
        {
            baristaAnimator.SetBool("wave", true); // Start the wave animation
            StartCoroutine(ResetWave());
        }
    }

    private IEnumerator ResetWave()
    {
        yield return new WaitForSeconds(waveDuration); // Wait for the animation duration
        if (baristaAnimator != null)
        {
            baristaAnimator.SetBool("wave", false); // Reset the animation
        }
    }
}
