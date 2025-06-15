using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public string triggerName = "MagicAttack2H";  // Nom du paramètre dans l'Animator

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animator = other.GetComponent<Animator>();
            SimplePlayerController controller = other.GetComponent<SimplePlayerController>();

            if (animator != null && controller != null)
            {
                controller.canMove = false;
                animator.SetTrigger(triggerName);
            }
        }
    }
}