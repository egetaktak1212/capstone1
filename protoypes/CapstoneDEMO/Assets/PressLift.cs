using UnityEngine;

public class PressLift : MonoBehaviour
{
    Animator animator;
    public bool pressed = false;


    void Start()
    {
        // Get the Animator component on this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (pressed)
        {
            animator.SetBool("isPressed", true);
        }

        if (!pressed)
        {
            animator.SetBool("isPressed", false);
        }
    }
}
