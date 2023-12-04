using System;
using System.Collections.Generic;
using NUnit.Framework;
using NationalElectionSystem;

namespace NationalElectionSystem.Tests
{
    public class NationalElectionSystemCorrectnessTests
    {
        private IElectionManager _electionManager;

        [SetUp]
        public void Setup()
        {
            this._electionManager = new ElectionManager();
        }

        [Test]
        public void ContainsCandidate_ShouldReturnTrue_WhenCandidateExists()
        {
            // Arrange
            var candidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Candidate",
                Party = "Republican"
            };
            
            this._electionManager.AddCandidate(candidate);

            // Act
            var result = this._electionManager.Contains(candidate);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void ContainsVoter_ShouldReturnTrue_WhenCandidateExists()
        {
            // Arrange
            var voter = new Voter
            {
                Id = Guid.NewGuid().ToString(),
                Age = 20,
                Name = "Candidate"
            };
            
            this._electionManager.AddVoter(voter);

            // Act
            var result = this._electionManager.Contains(voter);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void GetCandidates_ShouldReturnAllRegisteredCandidates()
        {
            // Arrange
            var candidates = new List<Candidate>();
            for (int i = 0; i < 5; i++)
            {
                var candidate = new Candidate
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Candidate-" + i,
                    Party = "Republican"
                };
            
                this._electionManager.AddCandidate(candidate);
                candidates.Add(candidate);
            }

            // Act
            var result = this._electionManager.GetCandidates();

            // Assert
            Assert.That(result, Is.EquivalentTo(candidates));
        }

        [Test]
        public void GetVoters_ShouldReturnAllRegisteredVoters()
        {
            // Arrange
            var voters = new List<Voter>();
            for (int i = 0; i < 5; i++)
            {
                var voter = new Voter
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = 20,
                    Name = "Candidate-" + i,
                };
            
                this._electionManager.AddVoter(voter);
                voters.Add(voter);
            }

            // Act
            var result = this._electionManager.GetVoters();

            // Assert
            Assert.That(result, Is.EquivalentTo(voters));
        }

        [Test]
        public void GetVotesForCandidate_ShouldReturnCorrectVotes()
        {
            // Arrange
            var candidate = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Candidate-",
                Party = "Republican"
            };
            
            this._electionManager.AddCandidate(candidate);

            var votes = 5;
            for (int i = 0; i < votes; i++)
            {
                var voter = new Voter
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = 20,
                    Name = "Candidate-" + i,
                };
            
                this._electionManager.AddVoter(voter);
                this._electionManager.Vote(voter.Id, candidate.Id);
            }

            // Act
            var result = this._electionManager.GetVotesForCandidate(candidate.Id);

            // Assert
            Assert.That(result, Is.EqualTo(votes));
        }

        [Test]
        public void GetWinner_ShouldReturnTheCandidateWithMostVotes_WhenThereIsOnlyOneWinner()
        {
            // Arrange
            var candidate1 = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Candidate1",
                Party = "Republican"
            };
            
            var candidate2 = new Candidate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Candidate2",
                Party = "Democrates"
            };
            
            this._electionManager.AddCandidate(candidate1);
            this._electionManager.AddCandidate(candidate2);

            for (int i = 0; i < 11; i++)
            {
                var voter = new Voter
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = 20,
                    Name = "Candidate-" + i,
                };
            
                this._electionManager.AddVoter(voter);

                var candidateId = i % 2 == 0 ? candidate1.Id : candidate2.Id;
                this._electionManager.Vote(voter.Id, candidateId);
            }

            // Act
            var result = this._electionManager.GetWinner();

            // Assert
            Assert.That(result, Is.EquivalentTo(new List<Candidate> { candidate1 }));
        }

        [Test]
        public void GetCandidatesByParty_ShouldReturnOnlyCandidatesForGivenParty()
        {
            // Arrange
            var expectedResult = new List<Candidate>();
            
            for (int i = 0; i < 20; i++)
            {
                var isEven = i % 2 == 0;
                var party = isEven ? "Republican" : "Democrates";
                var candidate = new Candidate
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Candidate" + i,
                    Party = party
                };

                if (isEven)
                {
                    expectedResult.Add(candidate);
                }

                this._electionManager.AddCandidate(candidate);
            }

            // Act
            var result = this._electionManager.GetCandidatesByParty("Republican");

            // Assert
            Assert.That(result, Is.EquivalentTo(expectedResult));
        }
    }
}