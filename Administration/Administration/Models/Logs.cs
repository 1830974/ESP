﻿namespace Administration.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime EntryTime { get; set; }
        public string Origin { get; set; }
        public string Description { get; set; }

        public Logs() { }

        public Logs(int id, DateTime entryTime, string origin, string description)
        {
            Id = id;
            EntryTime = entryTime;
            Origin = origin;
            Description = description;
        }
    }
}
