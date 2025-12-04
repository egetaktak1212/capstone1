using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossManager : BossManager
{


    [SerializeField] GameObject NotePreviewText;

    bool tutorialOver = false;





    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {

        messageEmpty.gameObject.SetActive(false);
        deadController.gameObject.SetActive(false);
        passedController.gameObject.SetActive(false);
        youWinPanel.gameObject.SetActive(false);
        Debug.Log("A");
        NotePreviewText.SetActive(false);

        turnMessages = bossDialogueAndMusicInfo.getMessages();

        songNames = bossDialogueAndMusicInfo.getSongNames();


        StartCoroutine(playGameSteps());

    }

    public override void notePressedCorrectly()
    {
        if (tutorialOver)
        {
            base.notePressedCorrectly();
        }
    }

    public override void noteNotPressedCorrectly()
    {
        if (tutorialOver)
        {
            base.noteNotPressedCorrectly();
        }
        else
        {
            PlayerInfo.instance.madeAMistake();
        }
    }
    public override void noteMispress()
    {
        if (tutorialOver)
        {
            base.noteMispress();
        }
        else
        {
            PlayerInfo.instance.madeAMistake();
        }
    }



    protected override IEnumerator playGameSteps()
    {
        tutorialOver = false;

        //wait for animations
        yield return new WaitUntil(() => starterAnimation.GetCurrentAnimatorStateInfo(0).IsName("PianoStill"));
        Debug.Log("anim over");
        yield return new WaitForSeconds(2f);

        //say some shit setting up the game
        List<string> SetUp = new List<string> { "If you're gonna make it out of this place, you're gonna need to learn a thing or two.", "You happen to be quite lucky. You've got everything you need right here.", "Me.", "Most people here rather fight than talk, so you're gonna need to press the right notes at the right time." };

        enemyDisplayManager.speakingDisplay();
        messageEmpty.gameObject.SetActive(true);
        messageEmpty.startMessages(SetUp);
        yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);
        enemyDisplayManager.defaultDisplay();


        //blue will show you where to place your hand to begin.

        NotePreviewText.SetActive(true);

        yield return StartCoroutine(notePreview.startNotePreview("stageOne"));

        NotePreviewText.SetActive(false);

        //press the correct keys when the game starts

        List<string> PressCorrectKeys = new List<string> { "You're gonna see notes going down the board. Press 'em exactly when they're over that line at the end.", "Each line corresponds to a key. You'll see, give it a try." };

        enemyDisplayManager.speakingDisplay();
        messageEmpty.gameObject.SetActive(true);
        messageEmpty.startMessages(PressCorrectKeys);
        yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);
        enemyDisplayManager.defaultDisplay();

        //play a quick game without taking damage

        yield return StartCoroutine(notePreview.startNotePreview("stageOne"));

        midiPlayer.MPTK_MidiName = "stageOne";
        midiPlayer.MPTK_Play();

        while (true)
        {
            //the player is gonna be stuck in here just playing.

            if (!midiPlayer.MPTK_IsPlaying && !areNotesInPlay())
            {
                break;
            }

            yield return null;
        }


        //you made this many mistakes. wanna try again?
        //this is your health, it will go down if you fuck up. this is your combo, itll go up if youre consistent with it.

        //enemy health as well. each successful hit does damage, and the higher your combo, the higher your damage to the enemy.
        //lets have a fair bout

        List<string> HealthAndCombo = new List<string> { "Every note you miss, it lets me damage you. Every note you hit lets you damage me.", "Now, if you keep hittin the right notes over and over, you'll build your combo meter.", "The higher your combo, the more damage each note'll do to your enemy.", "Enough wafflin, let's have a fair ol' fight." };

        enemyDisplayManager.speakingDisplay();
        messageEmpty.gameObject.SetActive(true);
        messageEmpty.startMessages(HealthAndCombo);
        yield return new WaitUntil(() => messageEmpty.gameObject.activeSelf == false);
        enemyDisplayManager.defaultDisplay();

        //tutorial done


        tutorialOver = true;

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

                if (EnemyInfo.instance.dead)
                {
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
        else
        {
            midiPlayer.MPTK_Stop();
            clearNotes();
            enemyDisplayManager.playerWinDisplay();
            Win();
            yield return new WaitForSeconds(3f);
            Time.timeScale = 0f;
        }
    }
}
