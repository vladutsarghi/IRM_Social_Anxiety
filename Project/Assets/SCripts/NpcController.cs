using UnityEngine;

public class NpcController : MonoBehaviour
{
    private Animator animator; 
    private bool hasTriggered = false; 

    void Start()
    {
        
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("MainCamera") && !hasTriggered)
        {
            hasTriggered = true; 
            animator.SetTrigger("needToShake"); 
        }
    }
}
