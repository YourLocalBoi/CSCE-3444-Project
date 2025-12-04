using UnityEngine;

public class CameraCutscene : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public Transform cameraTarget;          // Where the camera pans to
    public float panDuration = 3f;          // Seconds for pan
    public bool startOnTrigger = true;      // If true, start when player enters trigger

    [Header("References")]
    public topPlayerMovement topPlayerMovement;
    public TopDownCameraFollow CameraFollow;
    public GameObject dialoguePanel;    //didn't actually do dialogue     

    private bool hasPlayed = false;

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void BeginCutscene()
    {
        Debug.Log("BeginCutscene called");
        if (hasPlayed) return;
        hasPlayed = true;
        StartCoroutine(PanRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!startOnTrigger) return;
        if (collision.CompareTag("Player"))
        {
            BeginCutscene();
        }
    }

    private System.Collections.IEnumerator PanRoutine()
    {
        Camera cam = Camera.main;
        if (cam == null || cameraTarget == null)
        {
            Debug.LogWarning("CameraCutscene: Missing camera or cameraTarget reference.");
            yield break;
        }

        // Disable player control and camera follow
        if (topPlayerMovement != null) topPlayerMovement.canMove = false;
        if (CameraFollow != null) CameraFollow.canFollow = false;

        Vector3 startPos = cam.transform.position;
        Vector3 endPos = new Vector3(
            cameraTarget.position.x,
            cameraTarget.position.y,
            cam.transform.position.z // keep the same Z
        );

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / panDuration;
            float eased = Mathf.SmoothStep(0f, 1f, t); //smooth easing
            cam.transform.position = Vector3.Lerp(startPos, endPos, eased);
            yield return null;
        }

        // Ensure final position is exact
        cam.transform.position = endPos;




        // Re-enable player & camera follow
        if (topPlayerMovement != null) topPlayerMovement.canMove = true;
        if (CameraFollow != null) CameraFollow.canFollow = true;

    }
}

