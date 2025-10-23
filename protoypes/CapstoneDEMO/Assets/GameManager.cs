using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MidiPlayerTK;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int stepIndex = 1;

    bool songOver;

    public MessageController messageEmpty;
    public DeadController deadController;
    public PassedController passedController;
    public GameObject youWinPanel;

    public MidiFilePlayer midiPlayer;

    List<string> songNames = new List<string> {
        "stageOne",
        "stageTwo",
        "stageThree"
    };


    void Start()
    {
        messageEmpty.gameObject.SetActive(false);
        deadController.gameObject.SetActive(false);
        passedController.gameObject.SetActive(false);
        youWinPanel.gameObject.SetActive(false);
        StartCoroutine(playGameSteps());

    }

    // Update is called once per frame
    void Update()
    {





    }

    IEnumerator playGameSteps()
    {
        //initial game info
        messageEmpty.gameObject.SetActive(true);
        messageEmpty.startMessages(0);
        yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);

        while (true)
        {
            PlayerInfo.instance.resetLife();

            //make the mesages run and dont cont until we are done displaying messages
            messageEmpty.gameObject.SetActive(true);
            messageEmpty.startMessages(stepIndex);
            yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);

            midiPlayer.MPTK_MidiName = songNames[stepIndex-1];//i did minus one cuz i want the 0 to be the intro text
            midiPlayer.MPTK_Play();

           
            while (true)
            {
                //the player is gonna be stuck in here just playing.

                //if the player dies, break and send them back up to the top

                if (!midiPlayer.MPTK_IsPlaying)
                {
                    break;
                }
                else if (PlayerInfo.instance.dead) { 
                    midiPlayer.MPTK_Stop();
                    break;
                }

                yield return null;
            }
            if (PlayerInfo.instance.dead) {
                while (true) { 
                    deadController.gameObject.SetActive(true);
                    deadController.startMessages();
                    yield return new WaitUntil(() => deadController.gameObject.activeSelf == false);
                    break;
                }
                
            } else if (!midiPlayer.MPTK_IsPlaying) {
                yield return new WaitForSeconds(5f);
                while (true) {
                    passedController.gameObject.SetActive(true);
                    passedController.startMessages();
                    yield return new WaitUntil(() => passedController.gameObject.activeSelf == false);
                    break;
                }
                if (!passedController.trueIfRetryFalseIfContinue) {
                    stepIndex++;
                }
            }
            if (stepIndex == songNames.Count() + 1) {
                break;
            }
            //if we are out of steps break
        }
        youWinPanel.gameObject.SetActive(true);
        //you won good job       

    }



}
