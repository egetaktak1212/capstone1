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

    public static GameManager instance;

    public MessageController messageEmpty;
    public DeadController deadController;
    public PassedController passedController;
    public GameObject youWinPanel;

    [SerializeField] NotePreview notePreview;

    [SerializeField] Animator starterAnimation;

    [SerializeField] EnemyDisplayManager enemyDisplayManager;

    public MidiFilePlayer midiPlayer;

    public List<string> songNames = new List<string> {
        "stageOne",
        "stageTwo",
        "stageThree",
        "stageFour",
        "stageFive",
        "stageSix"
    };

    int songIndex = 0;

    public List<List<string>> turnMessages = new List<List<string>>
    {
        new List<string> { "Welcome to the game. This is just a basic tutorial.", "You're gonna love this trust me."},
        new List<string> { "For this first one, its just simple key presses!", "It shouldn't be hard. It's one note.","Just keep an eye on where its comin."},
        new List<string> { "This one is basic chord shapes but again, just simple notes.", "Try to use all of your fingers. Get your toes on there."},
        new List<string> { "Got some easy chords for you.", "I don't remember their names."},
        new List<string> { "Things are heating up.", "Like, it's faster now."},
        new List<string> { "This one's easier.", "I wanted to give you a break."},
        new List<string> { "Your final challenge.", "I have mixed chords with single notes.", "I'm progressive like that."}
    };

    int messageIndex = 0;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }
    }
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



    public void notePressedCorrectly() {
        Debug.Log("PRESSED CORRECTLY");
        ComboManager.instance.noteHappened(true);
        PlayerInfo.instance.increaseHealth(3);
        EnemyInfo.instance.dealDamage();
    }

    public void noteNotPressedCorrectly() {
        Debug.Log("YOU DIDNT PRESS");
        PlayerInfo.instance.madeAMistake();
        PlayerInfo.instance.decreaseHealth(15);
    }


    IEnumerator playGameSteps()
    {
        //wait for animations
        yield return new WaitUntil(() => starterAnimation.GetCurrentAnimatorStateInfo(0).IsName("PianoStill"));
        Debug.Log("anim over");
        yield return new WaitForSeconds(2f);
        while (true)
        {
            clearNotes();

            enemyDisplayManager.speakingDisplay();
            //make the mesages run and dont cont until we are done displaying messages
            messageEmpty.gameObject.SetActive(true);
            messageEmpty.startMessages(getMessageToDisplay());
            yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);
            enemyDisplayManager.defaultDisplay();

            yield return StartCoroutine(notePreview.startNotePreview(getSongToPlay()));

            midiPlayer.MPTK_MidiName = getSongToPlay();
            midiPlayer.MPTK_Play();



            while (true)
            {
                //the player is gonna be stuck in here just playing.

                //if the player dies, break and send them back up to the top

                if (EnemyInfo.instance.dead) {
                    break;
                }


                if (!midiPlayer.MPTK_IsPlaying && !areNotesInPlay())
                {
                    break;
                }
                else if (PlayerInfo.instance.dead)
                {
                    midiPlayer.MPTK_Stop();
                    break;
                }

                yield return null;
            }
            if (PlayerInfo.instance.dead)
            {
                enemyDisplayManager.playerLoseDisplay();
                while (true)
                {
                    deadController.gameObject.SetActive(true);
                    deadController.startMessages();
                    yield return new WaitUntil(() => deadController.gameObject.activeSelf == false);
                    break;
                }

            }
            else if (EnemyInfo.instance.dead)
            {
                enemyDisplayManager.playerWinDisplay();
                yield return new WaitForSeconds(2f);
                //while (true)
                //{
                //    passedController.gameObject.SetActive(true);
                //    passedController.startMessages();
                //    yield return new WaitUntil(() => passedController.gameObject.activeSelf == false);
                //    break;
                //}
                //if (!passedController.trueIfRetryFalseIfContinue)
                //{
                //    stepIndex++;
                //}
            }
            //if (stepIndex == songNames.Count())
            //{
            //    break;
            //}
            //if we are out of steps break
        }
        youWinPanel.gameObject.SetActive(true);
        //you won good job       

    }

    GameObject[] getNotesInPlay()
    {
        return GameObject.FindGameObjectsWithTag("NoteObject");
    }

    bool areNotesInPlay()
    {
        GameObject[] listOfNotes = getNotesInPlay();
        return listOfNotes.Length > 0;
    }

    void clearNotes()
    {
        GameObject[] listOfNotes = getNotesInPlay();
        foreach (GameObject note in listOfNotes)
        {
            Destroy(note);
        }
    }

    string getSongToPlay()
    {
        if (songIndex >= songNames.Count()) {
            songIndex = 0;
        }
        songIndex++;
        return songNames[songIndex-1];

    }

    List<string> getMessageToDisplay()
    {
        if (messageIndex >= turnMessages.Count())
        {
            messageIndex = songNames.Count()-1;
        }

        messageIndex++;

        return turnMessages[messageIndex-1];

    }


}
