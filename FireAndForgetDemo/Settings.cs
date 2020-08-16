namespace FireAndForgetDemo
{
    public class Settings
    {
        public string AwaitableTaskName { get; set; }
        public string FireAndForgetTaskName { get; set; }
        public int AwaitableTaskDurationInSeconds { get; set; }
        public int FireAndForgetTaskDurationInSeconds { get; set; }
        public bool FoundDocumentsOnUnstableStorage { get; set; }
    }
}
