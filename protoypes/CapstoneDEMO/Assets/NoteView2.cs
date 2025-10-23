using MidiPlayerTK;
using MPTK.NAudio.Midi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class NoteView2 : MonoBehaviour
{
    public static bool FirstNotePlayed = false;
    public MPTKEvent noteOn;
    public MidiStreamPlayer midiStreamPlayer;
    public bool played = false;
    public Material MatPlayed;
    public float zOriginal;
    bool condition = true;

    List<int> notesPressed = new List<int>();

    private void Awake()
    {
        //MidiInputs.instance.upcomingNotes.Add(noteOn.Value);
    }

    private void OnDestroy()
    {
        //MidiInputs.instance.supposedToPress.Remove(noteOn.Value);
        //MidiInputs.instance.upcomingNotes.Contains(noteOn.Value);
        //MidiInputs.instance.upcomingNotes.Remove(noteOn.Value);
        //MidiInputs.instance.upcomingNotes.Contains(noteOn.Value);

    }

    //-1.062
    public void Update()
    {
        // The midi event is played with a MidiStreamPlayer when position X < -45 (falling)
        if (!played && transform.position.z >= -2.327) //we are in range press
        {
            //when we are in range, append the value of the note to an array in the singleton, when we leave range remove it

            //if (!MidiInputs.instance.supposedToPress.Contains(noteOn.Value)) {
            //    MidiInputs.instance.supposedToPress.Add(noteOn.Value);
            //}
            getNotesPressed();
            if (isOurNoteBeingPressed(notesPressed, noteOn.Value)) //if we pressed
            {

                played = true;

                midiStreamPlayer.MPTK_PlayEvent(noteOn);

                FirstNotePlayed = true;

                gameObject.GetComponent<Renderer>().material = MatPlayed;// .color = Color.red;

                Debug.Log("PRESSED CORRECTLY");
                PlayerInfo.instance.increaseHealth(3);
                Destroy(this.gameObject);
                
            }
        }
        if (!played && transform.position.z >= -0.8) {
            Debug.Log("YOU DIDNT PRESS");
            PlayerInfo.instance.decreaseHealth(15);
            Destroy(this.gameObject);
            

        }
    }

    public void getNotesPressed() {
        notesPressed = MidiInputs.instance.getPressed();
    }



    bool isOurNoteBeingPressed(List<int> notes, int value) {


        return notes.Contains(value);
    }

    

    void FixedUpdate()
    {
        // Move the note along the X axis
        float translation = Time.fixedDeltaTime * NoteCreator.Speed / 2f;
        transform.Translate(0, 0, translation);
    }
}

