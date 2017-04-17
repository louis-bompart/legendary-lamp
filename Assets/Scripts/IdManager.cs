using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class IDManager
{
    private List<int> usedIDs;
    private Queue<int> freedIDs;
    private int nextID;
    private int currentMax;

    public IDManager()
    {
        usedIDs = new List<int>();
        freedIDs = new Queue<int>();
        nextID = 0;
        currentMax = 0;
    }

    public int GetNewID()
    {
        int toReturn = nextID;

        usedIDs.Add(nextID);
        if (freedIDs.Count == 0)
        {
            currentMax++;
            nextID = currentMax;
        }
        else
        {
            nextID = freedIDs.Dequeue();
        }
        return toReturn;
    }

    public void FreeID(int id2free)
    {
        freedIDs.Enqueue(id2free);
        usedIDs.Remove(id2free);
    }
}

