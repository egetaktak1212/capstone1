using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadController : MonoBehaviour
{

    Coroutine tryAgainCoroutine;

    bool waitingForButtonPress = false;

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
        if (tryAgainCoroutine != null)
        {
            StopCoroutine(tryAgainCoroutine);
        }

        waitingForButtonPress = true;

        tryAgainCoroutine = StartCoroutine(deadCoroutine());

    }

    IEnumerator deadCoroutine()
    {

        while (waitingForButtonPress)
        {
            yield return null;
        }

        tryAgainCoroutine = null;

        gameObject.SetActive(false);

    }


    public void tryAgainPressed()
    {
        if (waitingForButtonPress)
        {
            waitingForButtonPress = false;
        }
    }


}