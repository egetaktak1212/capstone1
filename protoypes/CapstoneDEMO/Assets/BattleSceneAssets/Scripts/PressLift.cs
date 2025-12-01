using System.Linq;
using UnityEngine;

public class PressLift : MonoBehaviour
{
    Animator animator;
    public bool pressed = false;
    public Material regularMat;
    public Material upcomingMat;
    public Material pressedMat;

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
            //GetComponent<Renderer>().material = pressedMat;
        }else if (!pressed)
        {
            animator.SetBool("isPressed", false);
            //GetComponent<Renderer>().material = regularMat;
        }
        if (MidiInputs.instance.upcomingNotes.Contains(int.Parse(gameObject.name)))
        {
            //GetComponent<Renderer>().material = upcomingMat;
        }
    }
}
