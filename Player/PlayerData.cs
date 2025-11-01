using System;
using UnityEngine;


namespace Transporter.Data
{
    [Serializable]
    public class PlayerData
    {
        public string playerName = "Captain";
        public float credits = 1000f;
        public Vector3 position = Vector3.zero;
        // Extend later: rotation, perks, reputation, inventory snapshot, etc.
    }
}
