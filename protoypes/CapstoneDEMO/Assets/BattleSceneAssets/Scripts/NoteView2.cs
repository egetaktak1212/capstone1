using MidiPlayerTK;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class NoteView2 : MonoBehaviour
{
    public static bool FirstNotePlayed = false;
    public MPTKEvent noteOn;
    public MidiStreamPlayer midiStreamPlayer;
    public bool played = false;
    public float zOriginal;
    bool condition = true;

    [SerializeField] GameObject rangeStart;

    [SerializeField] GameObject rangeStop;

    [SerializeField] Material blackChipMaterial;

    [SerializeField] Material redChipMaterial;





    List<int> notesPressed = new List<int>();

    int noteValue;

    private void Awake()
    {
        noteValue = normalizeNoteValue(noteOn.Value);
        MidiInputs.instance.upcomingNotes.Add(noteValue);
        int[] sharps = { 49, 51, 54, 56, 58, 61, 63, 66, 68, 70 };
        if (sharps.Contains(noteValue))
        {
            gameObject.GetComponent<Renderer>().material = blackChipMaterial;
        }
        else {
            gameObject.GetComponent<Renderer>().material = redChipMaterial;
        }

    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        MidiInputs.instance.supposedToPress.Remove(noteValue);
        //MidiInputs.instance.upcomingNotes.Contains(noteOn.Value);
        MidiInputs.instance.upcomingNotes.Remove(noteValue);
        //MidiInputs.instance.upcomingNotes.Contains(noteOn.Value);
        MidiInputs.instance.validPresses.Remove(noteValue);




    }

    //-1.062
    public void Update()
    {
        // The midi event is played with a MidiStreamPlayer when position X < -45 (falling)
        if (!played && transform.position.z >= rangeStart.transform.position.z) //we are in range press
        {
            //when we are in range, append the value of the note to an array in the singleton, when we leave range remove it
            
            if (!MidiInputs.instance.supposedToPress.Contains(noteValue)) {
                MidiInputs.instance.supposedToPress.Add(noteValue);
            }
            getNotesPressed();
            //Debug.Log(isOurNoteBeingPressed(notesPressed, noteOn.Value));
            if (isOurNoteBeingPressed(MidiInputs.instance.validPresses, noteValue)) //if we pressed
            {

                played = true;

                midiStreamPlayer.MPTK_PlayEvent(noteOn);

                FirstNotePlayed = true;


                GameManager.instance.notePressedCorrectly();
                Destroy(this.gameObject);
                
            }
        }
        if (!played && transform.position.z >= rangeStop.transform.position.z) {
            GameManager.instance.noteNotPressedCorrectly();
            Destroy(this.gameObject);
            

        }
    }

    public void getNotesPressed() {
        notesPressed = MidiInputs.instance.getPressed();
    }



    bool isOurNoteBeingPressed(List<int> notes, int value) {


        return notes.Contains(value);
    }

    int normalizeNoteValue(int value)
    {
        return (value % 24) + 48;
    }


    void FixedUpdate()
    {
        // Move the note along the X axis
        float translation = Time.fixedDeltaTime * NoteCreator.Speed / 2f;
        transform.Translate(0, 0, translation);
    }
}

