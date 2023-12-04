using System;
using System.Collections.Generic;
using NUnit.Framework;
using GitHubSystem;

namespace GitHubSystem.Tests
{
    public class CorrectnessTests
    {
        private IGitHubManager _manager;
        
        [SetUp]
        public void Setup()
        {
            this._manager = new GitHubManager();
        }

        [Test]
        public void CreateUser_AddsUser()
        {
            // Act
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
            };
            
            // Act
            this._manager.Create(user);
            
            Assert.True(this._manager.Contains(user));
        }

        [Test]
        public void CreateRepository_AddsRepository()
        {
            // Act
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
            };

            var repository = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            // Act
            this._manager.Create(user);
            this._manager.Create(repository);
            
            Assert.True(this._manager.Contains(repository));
        }
        
        [Test]
        public void ForkRepository_ReturnsNewRepository()
        {
            // Act
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
            };

            var repository = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            this._manager.Create(user);
            this._manager.Create(repository);

            // Act
            var forkedRepository = this._manager.ForkRepository(repository.Id, user.Id);
            
            Assert.True(this._manager.Contains(forkedRepository));
            Assert.That(forkedRepository.Name, Is.EqualTo(repository.Name));
            Assert.That(forkedRepository.OwnerId, Is.EqualTo(user.Id));
            Assert.That(forkedRepository.Stars, Is.Zero);
            Assert.That(forkedRepository.Id, Is.Not.EqualTo(repository.Id));
        }
        
        [Test]
        public void GetCommitsForRepository_ReturnsCommitsOnlyForGivenRepository()
        {
            // Act
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
            };

            var repository1 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            var repository2 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            this._manager.Create(user);
            this._manager.Create(repository1);
            this._manager.Create(repository2);

            // Act
            var commit1 = new Commit
            {
                Id = Guid.NewGuid().ToString(),
                Message = "Init commit",
                RepositoryId = repository1.Id,
                Timestamp = DateTime.Now.Millisecond,
                UserId = user.Id
            };
            this._manager.CommitChanges(commit1);

            var commit2 = new Commit
            {
                Id = Guid.NewGuid().ToString(),
                Message = "Init commit",
                RepositoryId = repository2.Id,
                Timestamp = DateTime.Now.Millisecond,
                UserId = user.Id
            };
            this._manager.CommitChanges(commit2);

            var repository1Commits = this._manager.GetCommitsForRepository(repository1.Id);
            var repository2Commits = this._manager.GetCommitsForRepository(repository2.Id);
            
            Assert.That(repository1Commits, Is.EquivalentTo(new List<Commit> { commit1 }));
            Assert.That(repository2Commits, Is.EquivalentTo(new List<Commit> { commit2 }));
        }
        
        [Test]
        public void GetMostForkedRepositories_ReturnsRepositoriesOrderedByNumOfForksInDescending()
        {
            // Act
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
            };

            var repository1 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            var repository2 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            this._manager.Create(user);
            this._manager.Create(repository1);
            this._manager.Create(repository2);

            // Act
            var forkedRepository = this._manager.ForkRepository(repository2.Id, user.Id);

            var result = this._manager.GetMostForkedRepositories();
            
            Assert.That(result, Is.EquivalentTo(new List<Repository> { repository2, repository1, forkedRepository }));
        }
        
        [Test]
        public void GetRepositoriesOrderedByCommitsInDescending_ReturnsRepositoriesOrderedByNumOfCommitsInDescending()
        {
            // Act
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user@gmail.com",
            };

            var repository1 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            var repository2 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user.Id,
                Stars = 100,
            };
            
            this._manager.Create(user);
            this._manager.Create(repository1);
            this._manager.Create(repository2);

            // Act
            var commit = new Commit
            {
                Id = Guid.NewGuid().ToString(),
                Message = "Init commit",
                RepositoryId = repository2.Id,
                Timestamp = DateTime.Now.Millisecond,
                UserId = user.Id
            };

            this._manager.CommitChanges(commit);

            var result = this._manager.GetRepositoriesOrderedByCommitsInDescending();
            
            Assert.That(result, Is.EquivalentTo(new List<Repository> { repository2, repository1 }));
        }
        
        [Test]
        public void GetRepositoriesByOwner_ReturnsRepositoriesCreatedByUser()
        {
            // Act
            var user1 = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user1@gmail.com",
            };
            
            var user2 = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = "user2@gmail.com",
            };

            var repository1 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user1.Id,
                Stars = 100,
            };
            
            var repository2 = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                OwnerId = user2.Id,
                Stars = 100,
            };
            
            this._manager.Create(user1);
            this._manager.Create(user2);
            this._manager.Create(repository1);
            this._manager.Create(repository2);

            // Act
            var result = this._manager.GetRepositoriesByOwner(user1.Id);
            
            // Assert
            Assert.That(result, Is.EquivalentTo(new List<Repository> { repository1 }));
        }
    }
}
