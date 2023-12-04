namespace GitHubSystem
{
    public class Commit
    {
        public string Id { get; set; }

        public string RepositoryId { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public long Timestamp { get; set; }
    }
}