using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using GitHubSystem;

namespace GitHubSystem.Tests
{
    public class PerformanceTests
    {
        private IGitHubManager _manager;
        
        [SetUp]
        public void Setup()
        {
            this._manager = new GitHubManager();
        }

        [Test]
        public void ContainsUser_ShouldPassQuickly_With10000000Users()
        {
            // Arrange
            var target = null as User;
            for (int i = 0; i < 1_000_000; i++)
            {
                var user = new User
                {
                    Id = i.ToString(),
                    Email = $"user{i}@gmail.com",
                    Username = $"User{i}"
                };

                if (i == 500_000)
                {
                    target = user;
                }
                
                this._manager.Create(user);
            }
            
            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._manager.Contains(target);
            
            sw.Stop();
            
            Assert.True(result);
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
        }

        [Test]
        public void ContainsRepository_ShouldPassQuickly_With10000000Repositories()
        {
            // Arrange
            var target = null as Repository;
            for (int i = 0; i < 1_000_000; i++)
            {
                var user = new User
                {
                    Id = i.ToString(),
                    Email = $"user{i}@gmail.com",
                    Username = $"User{i}"
                };

                var repository = new Repository
                {
                    Id = i.ToString(),
                    Name = i.ToString(),
                    OwnerId = user.Id,
                    Stars = 0,
                };

                if (i == 500_000)
                {
                    target = repository;
                }
                
                this._manager.Create(user);
                this._manager.Create(repository);
            }
            
            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._manager.Contains(target);
            
            sw.Stop();
            
            Assert.True(result);
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
        }

        [Test]
        public void GetCommitsForRepository_ShouldPassQuickly_With1000000Commits()
        {
            // Arrange
            var repositories = new List<Repository>();
            var users = new List<User>();
            
            for (int i = 0; i < 5; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = i.ToString(),
                    Email = $"user-{i}@abv.bg",
                };
                
                var repository = new Repository
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = i.ToString(),
                    Stars = 0,
                    OwnerId = user.Id
                };
                
                this._manager.Create(user);
                this._manager.Create(repository);
                
                repositories.Add(repository);
                users.Add(user);
            }

            var expectedCommits = 0;

            var random = new Random();
            for (int i = 0; i < 1_000_000; i++)
            {
                var idx = random.Next(0, repositories.Count);
                var commit = new Commit
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = "message" + i,
                    RepositoryId = repositories[idx].Id,
                    UserId = users[idx].Id,
                    Timestamp = DateTime.Now.Millisecond,
                };
                
                this._manager.CommitChanges(commit);

                if (idx == 0)
                {
                    expectedCommits += 1;
                }
            }
            
            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._manager.GetCommitsForRepository(repositories[0].Id);
            
            sw.Stop();
            
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.That(result.Count(), Is.EqualTo(expectedCommits));
        }

        [Test]
        public void GetRepositoriesByOwner_ShouldPassQuickly_With100_000UsersAnd1_000_000Repositories()
        {
            // Arrange
            for (int i = 0; i < 100_000; i++)
            {
                var user = new User
                {
                    Id = i.ToString(),
                    Username = i.ToString(),
                    Email = $"user-{i}@abv.bg",
                };
                
                this._manager.Create(user);
            }

            var random = new Random();
            var targetUserId = "123";
            var counter = 0;
            for (int i = 0; i < 1_000_000; i++)
            {
                var isTargetIteration = i % 10 == 0;
                var userId = isTargetIteration 
                    ? targetUserId
                    : random.Next(0, 100_000).ToString();

                if (userId == targetUserId)
                {
                    counter += 1;
                }
                
                var repository = new Repository
                {
                    Id = i.ToString(),
                    Name = i.ToString(),
                    OwnerId = userId,
                    Stars = 5,
                };
                
                this._manager.Create(repository);
            }
            
            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._manager.GetRepositoriesByOwner(targetUserId);
            
            sw.Stop();
            
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.That(result.Count(), Is.EqualTo(counter));
        }
        
        [Test]
        public void ForkRepository_ShouldPassQuickly_With100_000UsersAnd1_000_000Repos()
        {
            // Arrange
            for (int i = 0; i < 100_000; i++)
            {
                var user = new User
                {
                    Id = i.ToString(),
                    Username = i.ToString(),
                    Email = $"user-{i}@abv.bg",
                };
                
                this._manager.Create(user);
            }

            var random = new Random();
            var targetRepo = null as Repository;
            for (int i = 0; i < 1_000_000; i++)
            {
                var userId = random.Next(0, 100_000).ToString();
                var repository = new Repository
                {
                    Id = i.ToString(),
                    Name = i.ToString(),
                    OwnerId = userId,
                    Stars = 5,
                };

                if (i == 500_000)
                {
                    targetRepo = repository;
                }
                
                this._manager.Create(repository);
            }

            var targetUserId = random.Next(0, 100_000).ToString();
            
            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var forked = this._manager.ForkRepository(targetRepo.Id, targetUserId);
            
            sw.Stop();
            
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.That(forked.Id, Is.Not.EqualTo(targetRepo.Id));
            Assert.That(forked.Name, Is.EqualTo(targetRepo.Name));
            Assert.That(forked.OwnerId, Is.EqualTo(targetUserId));
            Assert.That(forked.Stars, Is.Zero);
        }
    }
}