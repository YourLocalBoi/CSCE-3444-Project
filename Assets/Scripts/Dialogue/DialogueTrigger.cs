using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public CollisionDialogue dialogue; // reference to dialogue object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogue.TriggerDialogue();
        }
    }
}
