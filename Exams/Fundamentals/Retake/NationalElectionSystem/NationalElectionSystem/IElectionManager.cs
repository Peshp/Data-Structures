using System.Collections.Generic;

namespace NationalElectionSystem
{
    public interface IElectionManager
    {
        void AddCandidate(Candidate candidate);

        void AddVoter(Voter voter);

        bool Contains(Candidate candidate);

        bool Contains(Voter voter);

        IEnumerable<Candidate> GetCandidates();

        IEnumerable<Voter> GetVoters();

        void Vote(string voterId, string candidateId);

        int GetVotesForCandidate(string candidateId);

        IEnumerable<Candidate> GetWinner();

        IEnumerable<Candidate> GetCandidatesByParty(string party);
    }
}