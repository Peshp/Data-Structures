using System;
using System.Collections.Generic;
using System.Linq;

namespace NationalElectionSystem
{
    public class ElectionManager : IElectionManager
    {
        private Dictionary<string, Candidate> candidates = new Dictionary<string, Candidate>();
        private Dictionary<string, Voter> voters = new Dictionary<string, Voter>();
        private Dictionary<string, int> candidateVoteCounts = new Dictionary<string, int>();

        public void AddCandidate(Candidate candidate)
        {
            candidates[candidate.Id] = candidate;
            candidateVoteCounts[candidate.Id] = 0;
        }

        public void AddVoter(Voter voter)
        {
            voters[voter.Id] = voter;
        }

        public bool Contains(Candidate candidate)
        {
            return candidates.ContainsKey(candidate.Id);
        }

        public bool Contains(Voter voter)
        {
            return voters.ContainsKey(voter.Id);
        }

        public IEnumerable<Candidate> GetCandidates()
        {
            return candidates.Values;
        }

        public IEnumerable<Voter> GetVoters()
        {
            return voters.Values;
        }

        public void Vote(string voterId, string candidateId)
        {
            if (!voters.ContainsKey(voterId) || !candidates.ContainsKey(candidateId) || voters[voterId].Age < 18)
            {
                throw new ArgumentException();
            }

            candidateVoteCounts[candidateId]++;
        }

        public int GetVotesForCandidate(string candidateId)
        {
            if (candidates.ContainsKey(candidateId))
            {
                return candidateVoteCounts[candidateId];
            }

            return 0;
        }

        public IEnumerable<Candidate> GetWinner()
        {
            var maxVotes = candidateVoteCounts.Max(pair => pair.Value);

            var winners = candidates.Where(candidate => candidateVoteCounts[candidate.Key] == maxVotes)
                .Select(candidate => candidate.Value);

            if (winners.Any())
            {
                return winners;
            }

            return null;
        }

        public IEnumerable<Candidate> GetCandidatesByParty(string party)
        {
            return candidates.Values
                .Where(candidate => candidate.Party == party)
                .OrderByDescending(candidate => candidateVoteCounts[candidate.Id])
                .ThenBy(candidate => candidate.Id);
        }
    }
}