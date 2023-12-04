using System;
using System.Collections.Generic;
using System.Linq;

public class Olympics : IOlympics
{
    private List<Competitor> competitors = new List<Competitor>();
    private List<Competition> competitions = new List<Competition>();

    public void AddCompetition(int id, string name, int participantsLimit)
    {
        if (competitions.Any(c => c.Id == id))
            throw new ArgumentException();

        competitions.Add(new Competition(name, id, participantsLimit));
    }

    public void AddCompetitor(int id, string name)
    {
        if (competitors.Any(c => c.Name == name && c.Id == id))
            throw new ArgumentException();

        competitors.Add(new Competitor(id, name));
    }

    public void Compete(int competitorId, int competitionId)
    {
        var competitorToAdd = competitors.FirstOrDefault(c => c.Id == competitorId);
        var competitionToAdd = competitions.FirstOrDefault(c => c.Id == competitionId);

        if(competitionToAdd is null || competitorToAdd is null)
            throw new ArgumentException();

        competitionToAdd.Competitors.Add(competitorToAdd);
    }

    public int CompetitionsCount()
        => competitions.Count;

    public int CompetitorsCount()
     => competitors.Count();

    public bool Contains(int competitionId, Competitor comp)
        => competitions.FirstOrDefault(c => c.Id == competitionId).Competitors.Any(c => c.Id == comp.Id);

    public void Disqualify(int competitionId, int competitorId)
    {
        var competitorToAdd = competitors.FirstOrDefault(c => c.Id == competitorId);
        var competitionToAdd = competitions.FirstOrDefault(c => c.Id == competitionId);

        if (competitionToAdd is null || competitorToAdd is null)
            throw new ArgumentException();

        competitionToAdd.Competitors.Remove(competitorToAdd);
        competitorToAdd.TotalScore -= competitionToAdd.Score;
    }

    public IEnumerable<Competitor> FindCompetitorsInRange(long min, long max)
        => competitors.Where(c => c.TotalScore >= min && c.TotalScore <= max);

    public IEnumerable<Competitor> GetByName(string name)
    {
        if(!competitors.Any(c => c.Name == name))
            throw new ArgumentException();

        return competitors.Where(c => c.Name == name);
    }

    public Competition GetCompetition(int id)
        => competitions.FirstOrDefault(c => c.Id == id);

    public IEnumerable<Competitor> SearchWithNameLength(int min, int max)
        => (IEnumerable<Competitor>)competitions.Where(c => c.Name.Length >= min && c.Name.Length <= max);
}