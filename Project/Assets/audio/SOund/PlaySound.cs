using UnityEngine;

public class CameraClapTrigger : MonoBehaviour
{
    public AudioSource applauseAudioSource; 
    public string clappingTriggerTag = "Clapping"; 
    public string stopClappingTriggerTag = "stopClapping"; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(clappingTriggerTag)) 
        {
            if (applauseAudioSource != null && !applauseAudioSource.isPlaying)
            {
                applauseAudioSource.Play();
            }
        }

        if (other.CompareTag(stopClappingTriggerTag)) 
        {
            if (applauseAudioSource.isPlaying)
            {
                applauseAudioSource.Stop(); 
            }
        }
    }
}
