using UnityEngine;
using UnityEngine.Events;

public class GoombaStomp : MonoBehaviour
{
    public UnityEvent onStomp;

    private Animator anim;
    private Collider2D col;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnStompFromPlayer()
    {
        if (anim != null)
            anim.SetTrigger("Stomped");

        if (col != null)
            col.enabled = false;

        if (rb != null)
            rb.simulated = false;

        onStomp?.Invoke();
        Destroy(gameObject, 0.4f);
    }
}
