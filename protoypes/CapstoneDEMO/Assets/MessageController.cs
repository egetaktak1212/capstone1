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
    new List<string> { "Final one okay? This one's reaaal tough."},
    new List<string> { "I lied this one sucks."}
};

    Coroutine speakinMessages;

    int currentTurn;
    int currentMessage;
    bool waitingForButtonPress = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startMessages(int index) {
        if (speakinMessages != null) { 
            StopCoroutine(speakinMessages);
        }

        currentTurn = index;
        currentMessage = 0;

        speakinMessages = StartCoroutine(messageCoroutine());

    }

    IEnumerator messageCoroutine() { 
        List<string> thisTurnMessages = turnMessages[currentTurn];

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
