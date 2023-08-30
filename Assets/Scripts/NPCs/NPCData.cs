using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NPC
{
    [CreateAssetMenu(fileName = "NPC", menuName = "New NPC")]
    public class NPCData : ScriptableObject
    {
        [Header("Info")]
        public string displayName;
        public string description;

        public override string ToString()
        {
            return displayName;
        }
    }

    
}

