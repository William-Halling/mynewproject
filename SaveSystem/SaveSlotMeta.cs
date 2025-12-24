using System;
using UnityEngine;


namespace Transporter.Data
{
    [Serializable]
    public class SaveSlotMeta
    {
        public string slotId;              // e.g. "slot_001"
        public string displayName;         // e.g. "Captain - Day 3"
        public string sceneName;           // e.g. "Game_Main"
        public string version;             // Application.version
        public string timestampIso;        // DateTime.UtcNow.ToString("o")


        public float credits;
        public Vector3 playerPos;


        public int worldSeed;


        public static SaveSlotMeta FromGameData(string slotId, string displayName, GameData data)
        {
            return new SaveSlotMeta
            {
                slotId       = slotId,
                displayName  = displayName,
                sceneName    = data.sceneName,
                version      = data.version,
                timestampIso = DateTime.UtcNow.ToString("o"),

                credits      = data.playerData != null ? data.playerData.credits : 0f,
                playerPos    = data.playerData != null ? data.playerData.position : Vector3.zero,
                worldSeed    = data.worldData  != null ? data.worldData.seed : 0
            };
        }
    }
}
