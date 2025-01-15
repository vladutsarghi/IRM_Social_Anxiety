using UnityEngine;

public class NpcController : MonoBehaviour
{
    private Animator animator;          
    private AudioSource audioSource;   
    private bool hasTriggered = false;  

    public AudioClip welcomeClip;       
    private float audioDelay = 3.0f;    
    public OfficeSitOnChair sitController; 

    void Start()
    {
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !hasTriggered)
        {
            hasTriggered = true;

            animator.SetTrigger("needToShake");

            Invoke(nameof(PlayWelcomeAudio), audioDelay);

            if (sitController != null)
            {
                sitController.TriggerSitDown();
                Debug.LogWarning("SitController NPC!");
            }
            else
            {
                Debug.LogWarning("SitController nu este setat pe NPC!");
            }
        }
    }

    private void PlayWelcomeAudio()
    {
        if (welcomeClip != null)
        {
            audioSource.PlayOneShot(welcomeClip);
            Debug.Log("NPC: Welcome!");
        }
        else
        {
            Debug.LogWarning("Nu este atribuit un fișier audio pentru Welcome!");
        }
    }
}
