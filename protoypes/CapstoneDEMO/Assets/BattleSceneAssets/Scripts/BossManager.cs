using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MidiPlayerTK;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    public static BossManager instance;

    public MessageController messageEmpty;
    public DeadController deadController;
    public PassedController passedController;
    public GameObject youWinPanel;

    [SerializeField] protected NotePreview notePreview;

    [SerializeField] protected Animator starterAnimation;

    [SerializeField] protected Animator deathAnimation;

    [SerializeField] protected Animator winAnimation;

    [SerializeField] protected EnemyDisplayManager enemyDisplayManager;

    [SerializeField] protected BossDialogueAndMusicInfo bossDialogueAndMusicInfo;


    public MidiFilePlayer midiPlayer;

    //public List<string> songNames = new List<string> {
    //    "stageOne",
    //    "stageTwo",
    //    "stageThree",
    //    "stageFour",
    //    "stageFive",
    //    "stageSix"
    //};

    public List<string> songNames = new List<string>();

    protected int songIndex = 0;

    //public List<List<string>> turnMessages = new List<List<string>>
    //{
    //    new List<string> { "Welcome to the game. This is just a basic tutorial.", "You're gonna love this trust me."},
    //    new List<string> { "For this first one, its just simple key presses!", "It shouldn't be hard. It's one note.","Just keep an eye on where its comin."},
    //    new List<string> { "This one is basic chord shapes but again, just simple notes.", "Try to use all of your fingers. Get your toes on there."},
    //    new List<string> { "Got some easy chords for you.", "I don't remember their names."},
    //    new List<string> { "Things are heating up.", "Like, it's faster now."},
    //    new List<string> { "This one's easier.", "I wanted to give you a break."},
    //    new List<string> { "Your final challenge.", "I have mixed chords with single notes.", "I'm progressive like that."}
    //};

    public List<List<string>> turnMessages = new List<List<string>>();

    protected int messageIndex = 0;
    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }
    protected virtual void Start()
    {
        messageEmpty.gameObject.SetActive(false);
        deadController.gameObject.SetActive(false);
        passedController.gameObject.SetActive(false);
        youWinPanel.gameObject.SetActive(false);

        turnMessages = bossDialogueAndMusicInfo.getMessages();

        songNames = bossDialogueAndMusicInfo.getSongNames();

        StartCoroutine(playGameSteps());

    }

    // Update is called once per frame
    void Update()
    {

    }



    public virtual void notePressedCorrectly() {
        Debug.Log("PRESSED CORRECTLY");
        ComboManager.instance.noteHappened(true);
        PlayerInfo.instance.increaseHealth(3);
        EnemyInfo.instance.dealDamage();
    }

    public virtual void noteNotPressedCorrectly() {
        Debug.Log("YOU DIDNT PRESS");
        PlayerInfo.instance.madeAMistake();
        PlayerInfo.instance.decreaseHealth(15);
    }
    public virtual void noteMispress()
    {
        Debug.Log("MISPRESS");
        PlayerInfo.instance.madeAMistake();
        PlayerInfo.instance.decreaseHealth(5);
    }

    protected virtual IEnumerator playGameSteps()
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

            string songName = getSongToPlay();

            yield return StartCoroutine(notePreview.startNotePreview(songName));

            midiPlayer.MPTK_MidiName = songName;
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
                break;
            }
            else if (EnemyInfo.instance.dead)
            {
                break;
            }
        }
        if (PlayerInfo.instance.dead)
        {
            enemyDisplayManager.playerLoseDisplay();
            Died();
            yield return new WaitForSeconds(3f);
            Time.timeScale = 0f;
        }
        else {
            midiPlayer.MPTK_Stop();
            clearNotes();
            enemyDisplayManager.playerWinDisplay();
            Win();
            yield return new WaitForSeconds(3f);
            Time.timeScale = 0f;
        }
          

    }

    protected GameObject[] getNotesInPlay()
    {
        return GameObject.FindGameObjectsWithTag("NoteObject");
    }

    protected bool areNotesInPlay()
    {
        GameObject[] listOfNotes = getNotesInPlay();
        return listOfNotes.Length > 0;
    }

    protected void clearNotes()
    {
        GameObject[] listOfNotes = getNotesInPlay();
        foreach (GameObject note in listOfNotes)
        {
            Destroy(note);
        }
    }

    protected string getSongToPlay()
    {
        if (songIndex >= songNames.Count()) {
            songIndex = 0;
        }
        songIndex++;
        return songNames[songIndex-1];

    }

    protected List<string> getMessageToDisplay()
    {
        if (messageIndex >= turnMessages.Count())
        {
            messageIndex = turnMessages.Count()-1;
        }

        messageIndex++;

        return turnMessages[messageIndex-1];

    }

    protected void Died() {
        deathAnimation.SetTrigger("Died");
    }

    protected void Win()
    {
        winAnimation.SetTrigger("Died");
    }



}
