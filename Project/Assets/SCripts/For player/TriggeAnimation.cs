using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public CharacterStateController.CharacterState targetState;
    public GameObject[] characters;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")) 
        {
            foreach (GameObject character in characters)
            {
                CharacterStateController controller = character.GetComponent<CharacterStateController>();
                if (controller != null)
                {
                    controller.SetState(targetState);
                }
            }
        }
    }
}
