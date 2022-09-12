namespace BugTrackerAPI.Data
{
    // type of events list for logging purpose
    public class LogEvents
    {
        public const int GetItems = 1000;
        public const int GetItem = 1001;
        public const int InsertItem = 1002;
        public const int UpdateItem = 1003;
        public const int DeleteItem = 1004;

        public const int Login = 3000;

        public const int GetItemNotFound = 4000;
        public const int UpdateItemNotFound = 4001;

        public const int OtherException = 5000;
    }
}
