using System;
using System.Collections.Generic;
using UnityEngine;
using Transporter.World;



namespace Transporter.Contracts
{
    public sealed class ContractGenerator
    {
        private readonly System.Random rng = new System.Random();



        // Tune these later to fit your flavour
        private static readonly string[] Titles =
        {
            "Medical Supplies Delivery",
            "Priority Components",
            "Luxury Textiles",
            "Food Staples Shipment",
            "Mechanical Parts Order",
            "Confidential Documents"
        };



        private static readonly string[] Descriptions =
        {
            "Handle with care. Delivery is time-sensitive.",
            "High-priority parts for ongoing repairs.",
            "Luxuries for high-end clients. Discretion preferred.",
            "Bulk shipment for local warehouses.",
            "Technical equipment. Keep dry and secure.",
            "Official papers. Expect interest from customs."
        };



        public ContractData GenerateRandomContract(IReadOnlyList<Location> locations)
        {
            if (locations == null || locations.Count < 2)
            {
                return CreateFallbackContract();
            }


            int a = rng.Next(locations.Count);
            int b;
           
            
            do 
            {
                b = rng.Next(locations.Count); 
            } 
            while (b == a);



            var origin = locations[a];
            var dest = locations[b];

            int difficulty = rng.Next(1, 6); // 1–5
            int rewardBase = 200 + difficulty * 150 + rng.Next(0, 200);

            bool illegal = rng.NextDouble() < 0.12;
           
            
            if (illegal)
            {
                rewardBase = Mathf.RoundToInt(rewardBase * 1.5f);
            }




            // 50% chance of a time limit
            float timeLimit = (rng.NextDouble() < 0.5)
                ? 0f
                : Mathf.Lerp(300f, 3600f, (float)rng.NextDouble());

            int idx = rng.Next(Titles.Length);



            return new ContractData
            {
                id = Guid.NewGuid().ToString(),
                title = Titles[idx],
                description = Descriptions[idx],
                originId = origin.Id,
                originName = origin.Name,
                destinationId = dest.Id,
                destinationName = dest.Name,
                reward = rewardBase,
                timeLimitSeconds = timeLimit,
                difficulty = difficulty,
                cargoSize = 1 + difficulty,
                illegal = illegal,
                isAccepted = false,
                isCompleted = false,
                isExpired = false
            };
        }




        private static ContractData CreateFallbackContract()
        {
            return new ContractData
            {
                id = Guid.NewGuid().ToString(),
                title = "Fallback Delivery",
                description = "Temporary contract used when locations are unavailable.",
                originId = "UNK",
                originName = "Unknown Port",
                destinationId = "UNK",
                destinationName = "Unknown Port",
                reward = 100,
                timeLimitSeconds = 0f,
                difficulty = 1,
                cargoSize = 1,
                illegal = false
            };
        }
    }
}
