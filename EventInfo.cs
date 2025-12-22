using System;

namespace ClubManageApp
{
    // Class ?? l?u thông tin s? ki?n
    public class EventInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public EventInfo()
        {
            Id = 0;
            Title = "";
            Date = DateTime.Now;
            StartTime = "08:00";
            EndTime = "09:00";
            Description = "";
            Location = "";
        }

        public string GetTimeRange()
        {
            return $"{StartTime} - {EndTime}";
        }

        // Override ToString ?? hi?n th? ??p trong ListBox
        public override string ToString()
        {
            return $"{StartTime} - {Title}";
        }
    }
}
