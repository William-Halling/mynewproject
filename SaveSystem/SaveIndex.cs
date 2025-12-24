using System;
using System.Collections.Generic;
using System.Linq;

namespace Transporter.Data
{
    [Serializable]
    public class SaveIndex
    {
        public List<SaveSlotMeta> slots = new List<SaveSlotMeta>();


        public SaveSlotMeta Get(string slotId)
        {
            return slots.Find(s => s.slotId == slotId);
        }


        public SaveSlotMeta GetMostRecent()
        {
            if (slots == null || slots.Count == 0)
            {
                return null;
            }

            SaveSlotMeta mostRecent = null;
            DateTime newestTime = DateTime.MinValue;


            foreach (var slot in slots)
            {
                if (string.IsNullOrEmpty(slot.timestampIso))
                {
                    continue;
                }

                if (!DateTime.TryParse(slot.timestampIso, out var time))
                {
                    continue;
                }

                if (time > newestTime)
                {
                    newestTime = time;
                    mostRecent = slot;
                }
            }

            return mostRecent;
        }
        

        public void Upsert(SaveSlotMeta meta)
        {
            int i = slots.FindIndex(s => s.slotId == meta.slotId);
           
            if (i >= 0) 
            {
                slots[i] = meta;
            }
            else 
            {
                slots.Add(meta);
            }
        }


        public void Remove(string slotId)
        {
            slots.RemoveAll(s => s.slotId == slotId);
        }
    }
}
