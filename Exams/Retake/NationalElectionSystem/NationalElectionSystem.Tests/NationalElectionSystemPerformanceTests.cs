using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NationalElectionSystem;

namespace NationalElectionSystem.Tests
{
    public class NationalElectionSystemPerformanceTests
    {
        private IElectionManager _electionManager;

        [SetUp]
        public void Setup()
        {
            this._electionManager = new ElectionManager();
        }

        [Test]
        public void ContainsCandidate_ShouldReturnTrueQuickly_With1000000Candidates()
        {
            // Arrange
            var target = null as Candidate;
            for (int i = 0; i < 1000000; i++)
            {
                var candidate = new Candidate
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Candidate" + i,
                    Party = "Republican"
                };
                
                this._electionManager.AddCandidate(candidate);

                if (i == 600000)
                {
                    target = candidate;
                }
            }

            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._electionManager.Contains(target);

            sw.Stop();
            
            // Assert
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.True(result);
        }

        [Test]
        public void ContainsVoter_ShouldReturnTrueQuickly_With1000000Candidates()
        {
            // Arrange
            var target = null as Voter;
            for (int i = 0; i < 1000000; i++)
            {
                var voter = new Voter()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Voter" + i,
                    Age = 23,
                };
                
                this._electionManager.AddVoter(voter);

                if (i == 600000)
                {
                    target = voter;
                }
            }

            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._electionManager.Contains(target);

            sw.Stop();
            
            // Assert
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.True(result);
        }

        [Test]
        public void GetCandidatesByParty_ShouldPassQuickly_With1000000Candidates()
        {
            // Arrange
            var expectedCount = 0;
            for (int i = 0; i < 1000000; i++)
            {
                var isRepublican = i % 2 == 0;
                var candidate = new Candidate
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Candidate" + i,
                    Party = isRepublican ?  "Republican" : "Democrates",
                };
                
                this._electionManager.AddCandidate(candidate);

                expectedCount = isRepublican ? expectedCount + 1 : expectedCount;
            }

            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._electionManager.GetCandidatesByParty("Republican");

            sw.Stop();
            
            // Assert
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void GetWinner_ShouldPassQuickly_With1000000Voters()
        {
            // Arrange
            var republicanCandidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "RepublicanCandidate",
                Party = "Republican",
            };
            
            var democratesCandidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "DemocratesCandidate",
                Party = "Democrates",
            };
            
            this._electionManager.AddCandidate(republicanCandidate);
            this._electionManager.AddCandidate(democratesCandidate);
            
            
            for (int i = 0; i < 10000001; i++)
            {
                var voter = new Voter()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Voter" + i,
                    Age = 23,
                };
                
                this._electionManager.AddVoter(voter);

                var candidateId = i % 2 == 0 ? republicanCandidate.Id : democratesCandidate.Id;
                this._electionManager.Vote(voter.Id, candidateId);
            }

            // Act
            var sw = new Stopwatch();
            
            sw.Start();

            var result = this._electionManager.GetWinner();

            sw.Stop();
            
            // Assert
            Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
            Assert.That(result, Is.EquivalentTo(new List<Candidate> { republicanCandidate }));
        }

        [Test]
        public void GetVotesForCandidate_ShouldPassQuickly_With1000000Voters()
        {
            // Arrange
            var republicanCandidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "RepublicanCandidate",
                Party = "Republican",
            };
            
            var democratesCandidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "DemocratesCandidate",
                Party = "Democrates",
            };
            
            this._electionManager.AddCandidate(republicanCandidate);
            this._electionManager.AddCandidate(democratesCandidate);

            var republicansCount = 0;
            var democratesCount = 0;
            for (int i = 0; i < 10000001; i++)
            {
                var voter = new Voter()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Voter" + i,
                    Age = 23,
                };
                
                this._electionManager.AddVoter(voter);

                if (i % 2 == 0)
                {
                    this._electionManager.Vote(voter.Id, republicanCandidate.Id);
                    republicansCount += 1;
                }
                else
                {
                    this._electionManager.Vote(voter.Id, democratesCandidate.Id);
                    democratesCount += 1;
                }
            }

            // Act
            var expectedResults = new[]
            {
                new { CandidateId = democratesCandidate.Id, ExpectedCount = democratesCount },
                new { CandidateId = republicanCandidate.Id, ExpectedCount = republicansCount }
            };
            
            foreach (var expectedResult in expectedResults)
            {
                var sw = new Stopwatch();
            
                sw.Start();

                var result = this._electionManager.GetVotesForCandidate(expectedResult.CandidateId);

                sw.Stop();
            
                // Assert
                Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(10));
                Assert.That(result, Is.EqualTo(expectedResult.ExpectedCount));
            }
        }
    }
}