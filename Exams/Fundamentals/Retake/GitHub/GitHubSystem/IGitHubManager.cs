using System.Collections.Generic;

namespace GitHubSystem
{
    public interface IGitHubManager
    {
        void Create(User user);

        void Create(Repository repository);

        bool Contains(User user);

        bool Contains(Repository repository);

        void CommitChanges(Commit commit);

        Repository ForkRepository(string repositoryId, string userId);

        IEnumerable<Commit> GetCommitsForRepository(string repositoryId);

        IEnumerable<Repository> GetRepositoriesByOwner(string userId);

        IEnumerable<Repository> GetMostForkedRepositories();

        IEnumerable<Repository> GetRepositoriesOrderedByCommitsInDescending();
    }
}