namespace Investigations.Models.Cases;

public interface ICaseRepository
{
    Task<List<Case>> GetCases();
    Task<Case> GetCase(int caseKey);
    Task<int> AddCase(Case case_, int instertedByUserKey = 100);
    Task<int> UpdateCase(Case case_, int updatedByUserKey = 100);
    Task<int> DeleteCase(int caseKey);
}
