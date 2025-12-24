using System;



namespace Transporter.Contracts
{
    [Serializable]
    public class ContractData
    {
        public string id;

        public string title;
        public string description;

        public string originId;
        public string originName;

        public string destinationId;
        public string destinationName;

        public int reward;               // credits
        public float timeLimitSeconds;   // 0 = no time limit
        public int difficulty;           // 1–5
        public int cargoSize;            // abstract “units”

        public bool illegal;

        public bool isAccepted;
        public bool isCompleted;
        public bool isExpired;
    }
}
