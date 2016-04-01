using UnityEngine;

public class CartSprite : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer renderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        animator.speed = 0;
    }

    void Update()
    {
        float angle = (360 - transform.parent.rotation.eulerAngles.z);
        float frame = ((angle + 30f) % 360f) / 360f;
        animator.Play("Cart", -1, frame);

        if (angle > 185f && angle < 340f)
        {
            renderer.sortingOrder = -1;
        }
        else
        {
            renderer.sortingOrder = 1;
        }
    }

    void LateUpdate()
    {
        // Don't rotate with parent
        transform.localRotation = transform.localRotation * Quaternion.Inverse(transform.rotation);
    }
}
