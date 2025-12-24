using System;
using System.Collections.Generic;
using Transporter.World;
using UnityEngine;

namespace Transporter.Contracts
{
    public sealed class ContractManager : MonoBehaviour
    {
        public static ContractManager Instance { get; private set; }

        [Header("Limits")]
        [SerializeField] private int maxOfferedContracts = 6;
        [SerializeField] private int maxActiveContracts = 3;

        private readonly List<ContractData> offeredContracts = new();
        private readonly List<ContractData> activeContracts = new();

        private readonly ContractGenerator generator = new ContractGenerator();

        public IReadOnlyList<ContractData> Offered => offeredContracts;
        public IReadOnlyList<ContractData> Active => activeContracts;

        public event Action OffersChanged;
        public event Action ActiveContractsChanged;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);

                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            ServiceLocator.Instance?.RegisterService(this);
        }



        private void Start()
        {
            // First batch once world exists
            TryInitialRefresh();
        }



        private void TryInitialRefresh()
        {
            if (WorldManager.Instance == null)
            {
                Debug.LogWarning("[ContractManager] No WorldManager yet. Call RefreshOffers after world generation.");
              

                return;
            }

            RefreshOffers(maxOfferedContracts);
        }



        public void RefreshOffers(int desiredCount)
        {
            offeredContracts.Clear();

            if (WorldManager.Instance == null)
            {
                Debug.LogWarning("[ContractManager] Cannot refresh offers, WorldManager missing.");
                OffersChanged?.Invoke();

                return;
            }

            var locations = WorldManager.Instance.Locations;
            int count = Mathf.Clamp(desiredCount, 1, maxOfferedContracts);


            for (int i = 0; i < count; i++)
            {
                offeredContracts.Add(generator.GenerateRandomContract(locations));
            }


            OffersChanged?.Invoke();
        }




        public void AcceptContract(ContractData contract)
        {
            if (contract == null)
               
                return;


            if (!offeredContracts.Contains(contract))
            {
                Debug.LogWarning("[ContractManager] Tried to accept contract not in offers list.");
               
                return;
            }


            if (activeContracts.Count >= maxActiveContracts)
            {
                Debug.LogWarning("[ContractManager] Max active contracts reached.");
                
                return;
            }

            contract.isAccepted = true;

            offeredContracts.Remove(contract);
            activeContracts.Add(contract);


            OffersChanged?.Invoke();
            ActiveContractsChanged?.Invoke();


            GameEventBus.Instance?.Publish(GameEventType.ContractAccepted, contract);
        }



        public void AbandonContract(ContractData contract)
        {
            if (contract == null)
               
                return;


            if (!activeContracts.Contains(contract))
             
                return;

            activeContracts.Remove(contract);
            contract.isAccepted = false;
            contract.isCompleted = false;

            ActiveContractsChanged?.Invoke();
            GameEventBus.Instance?.Publish(GameEventType.ContractAbandoned, contract);
        }



        public void CompleteContract(ContractData contract)
        {
            if (contract == null || !activeContracts.Contains(contract))
                
                return;


            contract.isCompleted = true;
            activeContracts.Remove(contract);

            ActiveContractsChanged?.Invoke();
            GameEventBus.Instance?.Publish(GameEventType.ContractCompleted, contract);
        }




        public ContractData GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            { 
                return null;
            }


            foreach (var c in offeredContracts)
            {   
                if (c.id == id)
                {    
                    return c;
                }
            }


            foreach (var c in activeContracts)
            { 
                if (c.id == id)
                { 
                    return c;
                }
            }

            return null;
        }




        /// <summary>
        /// Optional timer ticking for time-limited jobs.
        /// Call this from WorldClock or Update if you want deadlines.
        /// </summary>
        public void TickContracts(float deltaTime)
        {
            bool anyChanged = false;

            foreach (var c in activeContracts)
            {
                if (c.isCompleted || c.isExpired)
                {     
                    continue;
                }


                if (c.timeLimitSeconds <= 0f)
                { 
                    continue;
                }

                c.timeLimitSeconds -= deltaTime;


                if (c.timeLimitSeconds <= 0f)
                {
                    c.timeLimitSeconds = 0f;
                    c.isExpired = true;
                    anyChanged = true;
                    GameEventBus.Instance?.Publish(GameEventType.ContractExpired, c);
                }
            }



            if (anyChanged)
            {
                ActiveContractsChanged?.Invoke();
            }
        }
    }
}
