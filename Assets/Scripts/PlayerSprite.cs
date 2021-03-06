using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private SpriteRenderer renderer;
    private float speed = 0;
    private string character;
    private string color;

    void Awake()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        body = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Player player = transform.parent.gameObject.GetComponent<Player>();
        int playerNumber = player.playerNumber;
        character = CharacterSelection.Get(playerNumber);
        if (playerNumber < 3)
        {
            color = "Red";
        }
        else
        {
            color = "Blue";
        }
    }

    void FixedUpdate()
    {
        FaceCart();

        SetRunningSpeed();

        DetectIdle();

        animator.SetFloat("myspeed", speed);
    }

    void LateUpdate()
    {
        // Don't rotate with parent
        transform.localRotation = transform.localRotation * Quaternion.Inverse(transform.rotation);
    }

    /*
     * Flip the player sprite to be facing 'cart direction'
     */
    private void FaceCart()
    {
        Quaternion parentRotation = transform.parent.transform.rotation;

        float zRot = parentRotation.eulerAngles.z;

        if (zRot > 90 && zRot < 270)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
    }

    /*
     * Switch between running and idle animation based on speed
     */
    private void DetectIdle()
    {
        string idleAnimation = character + "Idle" + color;
        string walkAnimation = character + "Walk" + color;

        if (!IsRunning())
        {
            speed = 0.25f;
            animator.Play(idleAnimation);
        }
        else
        {
            animator.Play(walkAnimation);
        }
    }

    /*
     * Set running animation speed based on speed
     */
    private void SetRunningSpeed()
    {
        speed = body.velocity.magnitude / 10f;

        if (RunningBackwards() && speed > 0)
        {
            speed *= -1;
        }
        else if (!RunningBackwards() && speed < 0)
        {
            speed *= -1;
        }
    }

    private bool IsRunning()
    {
        return body.velocity.magnitude > 1;
    }

    private bool RunningBackwards()
    {
        float xSpeed = body.velocity.x;
        return (renderer.flipX && xSpeed > 0 || !renderer.flipX && xSpeed < 0);
    }
}
