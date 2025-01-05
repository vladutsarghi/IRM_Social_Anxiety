using UnityEngine;

public class CharacterStateController : MonoBehaviour
{
    public enum CharacterState { Sitting, Lapping }
    private CharacterState currentState;

    public void SetState(CharacterState newState)
    {
        currentState = newState;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            if (currentState == CharacterState.Sitting)
                animator.SetTrigger("Sit");
            else if (currentState == CharacterState.Lapping)
                animator.SetTrigger("Lap");
        }
    }
}
