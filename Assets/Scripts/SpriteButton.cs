using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color inFocus = new(1, 1, 1, 1);
    private Color outOfFocus = new(1, 1, 1, 0.5f);

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        spriteRenderer.color = outOfFocus;
        animator.enabled = false;
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = inFocus;
        animator.enabled = true;
    }

    void OnMouseExit()
    {
        spriteRenderer.color = outOfFocus;
        animator.enabled = false;
    }

}
