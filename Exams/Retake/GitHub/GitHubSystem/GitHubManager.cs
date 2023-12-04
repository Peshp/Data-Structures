using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHubSystem
{
    public class GitHubManager : IGitHubManager
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();
        private Dictionary<string, Repository> repositories = new Dictionary<string, Repository>();
        private Dictionary<string, List<Commit>> commitsByRepository = new Dictionary<string, List<Commit>>();
        private Dictionary<string, int> forkCounts = new Dictionary<string, int>();

        public void Create(User user)
        {
            users[user.Id] = user;
        }

        public void Create(Repository repository)
        {
            repositories[repository.Id] = repository;
            commitsByRepository[repository.Id] = new List<Commit>();
        }

        public bool Contains(User user)
        {
            return users.ContainsKey(user.Id);
        }

        public bool Contains(Repository repository)
        {
            return repositories.ContainsKey(repository.Id);
        }

        public void CommitChanges(Commit commit)
        {
            if (!repositories.TryGetValue(commit.RepositoryId, out var repository) || !users.ContainsKey(commit.UserId))
            {
                throw new ArgumentException();
            }

            commitsByRepository[commit.RepositoryId].Add(commit);
        }

        public Repository ForkRepository(string repositoryId, string userId)
        {
            if (!repositories.ContainsKey(repositoryId) || !users.ContainsKey(userId))
            {
                throw new ArgumentException();
            }

            var repositoryToClone = repositories[repositoryId];

            var newRepository = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = repositoryToClone.Name,
                OwnerId = userId,
                Stars = 0
            };

            repositories[newRepository.Id] = newRepository;
            commitsByRepository[newRepository.Id] = new List<Commit>();

            // Copy commits efficiently
            if (commitsByRepository.TryGetValue(repositoryId, out var commitsToCopy))
            {
                commitsByRepository[newRepository.Id].AddRange(commitsToCopy.Select(c =>
                    new Commit
                    {
                        Id = Guid.NewGuid().ToString(),
                        RepositoryId = newRepository.Id,
                        UserId = c.UserId,
                        Message = c.Message,
                        Timestamp = c.Timestamp
                    }));
            }

            // Increment the fork count of the original repository
            if (forkCounts.ContainsKey(repositoryId))
            {
                forkCounts[repositoryId]++;
            }
            else
            {
                forkCounts[repositoryId] = 1;
            }

            return newRepository;
        }

        public IEnumerable<Commit> GetCommitsForRepository(string repositoryId)
        {
            return commitsByRepository.GetValueOrDefault(repositoryId, new List<Commit>());
        }

        public IEnumerable<Repository> GetRepositoriesByOwner(string userId)
        {
            return repositories.Values.Where(r => r.OwnerId == userId);
        }

        public IEnumerable<Repository> GetMostForkedRepositories()
        {
            return repositories.Values.OrderByDescending(r => forkCounts.GetValueOrDefault(r.Id, 0)).ThenBy(r => repositories.Values.ToList().IndexOf(r));
        }

        public IEnumerable<Repository> GetRepositoriesOrderedByCommitsInDescending()
        {
            return repositories.Values.OrderByDescending(r => forkCounts.GetValueOrDefault(r.Id, 0)).ThenBy(r => repositories.Values.ToList().IndexOf(r));
        }
    }
}