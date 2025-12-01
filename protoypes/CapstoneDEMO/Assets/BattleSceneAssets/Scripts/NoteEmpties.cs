using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NoteEmpties : MonoBehaviour
{
    private Dictionary<int, Transform> notePositions = new Dictionary<int, Transform>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            string objName = child.name;
            string numberPart = objName.Substring(0, 2);

            int noteNumber = int.Parse(numberPart);
            notePositions[noteNumber] = child;
            
        }
    }

    public Vector3 GetNotePosition(int note)
    {
        return notePositions[note].transform.position;
    }
}
