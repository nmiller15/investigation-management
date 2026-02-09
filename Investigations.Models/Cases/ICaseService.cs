using Investigations.Models.Shared;

namespace Investigations.Models.Cases;

public interface ICaseService
{
    Task<MethodResponse<List<Case>>> GetCases();
    Task<MethodResponse<Case>> GetCase(int caseKey);
    Task<MethodResponse<int>> Save(Case case_);
    Task<MethodResponse<int>> Delete(int caseKey);
}
