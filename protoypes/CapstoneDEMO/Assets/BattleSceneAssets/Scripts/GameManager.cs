using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MidiPlayerTK;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int stepIndex = 0;

    bool songOver;

    public MessageController messageEmpty;
    public DeadController deadController;
    public PassedController passedController;
    public GameObject youWinPanel;

    [SerializeField] NotePreview notePreview;

    [SerializeField] Animator starterAnimation;

    [SerializeField] EnemyDisplayManager enemyDisplayManager;

    public MidiFilePlayer midiPlayer;

    List<string> songNames = new List<string> {
        "stageOne",
        "stageTwo",
        "stageThree",
        "stageFour",
        "stageFive",
        "stageSix"
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

    //IEnumerator startAnimation() {
    //    yield return new WaitUntil(() => introAnimOver == false);
    //}






    IEnumerator playGameSteps()
    {
        //wait for animations
        yield return new WaitUntil(() => starterAnimation.GetCurrentAnimatorStateInfo(0).IsName("PianoStill"));
        Debug.Log("anim over");
        yield return new WaitForSeconds(2f);
        while (true)
        {
            clearNotes();
            PlayerInfo.instance.resetLife();

            enemyDisplayManager.speakingDisplay();
            //make the mesages run and dont cont until we are done displaying messages
            messageEmpty.gameObject.SetActive(true);
            messageEmpty.startMessages(stepIndex);
            yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);
            enemyDisplayManager.defaultDisplay();

            yield return StartCoroutine(notePreview.startNotePreview(songNames[stepIndex]));

            midiPlayer.MPTK_MidiName = songNames[stepIndex];
            midiPlayer.MPTK_Play();

            
           
            while (true)
            {
                //the player is gonna be stuck in here just playing.

                //if the player dies, break and send them back up to the top

                if (!midiPlayer.MPTK_IsPlaying && !areNotesInPlay())
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
                enemyDisplayManager.playerLoseDisplay();
                while (true) { 
                    deadController.gameObject.SetActive(true);
                    deadController.startMessages();
                    yield return new WaitUntil(() => deadController.gameObject.activeSelf == false);
                    break;
                }
                
            } else if (!midiPlayer.MPTK_IsPlaying) {
                enemyDisplayManager.playerWinDisplay();
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
            if (stepIndex == songNames.Count()) {
                break;
            }
            //if we are out of steps break
        }
        youWinPanel.gameObject.SetActive(true);
        //you won good job       

    }

    GameObject[] getNotesInPlay() {
        return GameObject.FindGameObjectsWithTag("NoteObject");
    }

    bool areNotesInPlay() {
        GameObject[] listOfNotes = getNotesInPlay();
        return listOfNotes.Length > 0;
    }

    void clearNotes() {
        GameObject[] listOfNotes = getNotesInPlay();
        foreach (GameObject note in listOfNotes) { 
            Destroy(note);
        }
    }



}
