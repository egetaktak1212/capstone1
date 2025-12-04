using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    List<List<string>> turnMessages = new List<List<string>>
{
    new List<string> { "Welcome to the game. This is just a basic tutorial.", "You're gonna love this trust me."},
    new List<string> { "For this first one, its just simple key presses!", "It shouldn't be hard. It's one note.","Just keep an eye on where its comin."},
    new List<string> { "This one is basic chord shapes but again, just simple notes.", "Try to use all of your fingers. Get your toes on there."},
    new List<string> { "Got some easy chords for you.", "I don't remember their names."},
    new List<string> { "Things are heating up.", "Like, it's faster now."},
    new List<string> { "This one's easier.", "I wanted to give you a break."},
    new List<string> { "Your final challenge.", "I have mixed chords with single notes.", "I'm progressive like that."}
};

    [SerializeField] Animator animator;


    Coroutine speakinMessages;

    int currentTurn;
    int currentMessage;
    bool waitingForButtonPress = false;

    private void OnEnable()
    {
        animator.SetTrigger("MessageAppear");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startMessages(List<string> index) {
        if (speakinMessages != null) { 
            StopCoroutine(speakinMessages);
        }

        //currentTurn = index;
        currentMessage = 0;

        speakinMessages = StartCoroutine(messageCoroutine(index));

    }

    IEnumerator messageCoroutine(List<string> index) { 
        List<string> thisTurnMessages = index;

        while (currentMessage < thisTurnMessages.Count) { 
            messageText.text = thisTurnMessages[currentMessage];

            waitingForButtonPress = true;
            yield return new WaitUntil(() => waitingForButtonPress == false);

            currentMessage++;
        
        }
        speakinMessages = null;

        gameObject.SetActive(false);

    }


    public void nextMessage() {
        if (waitingForButtonPress) { 
            waitingForButtonPress=false;
        }
    }


}
