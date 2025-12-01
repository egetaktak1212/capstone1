using System.Collections;
using TMPro;
using UnityEngine;

public class PassedController : MonoBehaviour
{

    Coroutine passedCoroutine;

    bool waitingForButtonPress = false;
    public bool trueIfRetryFalseIfContinue = false;

    [SerializeField] TextMeshProUGUI messageText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startMessages()
    {
        
        if (passedCoroutine != null)
        {
            StopCoroutine(passedCoroutine);
        }

        messageText.text = $"You passed. You made {PlayerInfo.instance.mistakeCounter} mistake(s). Do you want to play it again or continue?";

        waitingForButtonPress = true;

        passedCoroutine = StartCoroutine(deadCoroutine());

    }

    IEnumerator deadCoroutine()
    {

        while (waitingForButtonPress)
        {
            yield return null;
        }

        passedCoroutine = null;

        gameObject.SetActive(false);

    }


    public void tryAgainPressed()
    {
        if (waitingForButtonPress)
        {
            trueIfRetryFalseIfContinue = true;
            waitingForButtonPress = false;
        }
        Debug.Log("try again presed");
        
    }

    public void continuePressed()
    {
        if (waitingForButtonPress)
        {
            trueIfRetryFalseIfContinue = false;
            waitingForButtonPress = false;
        }
    }


}
