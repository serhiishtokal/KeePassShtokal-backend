namespace KeePassShtokal.AppCore.Helpers
{
    public class Status
    {
        public Status(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Status()
        {
            
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
