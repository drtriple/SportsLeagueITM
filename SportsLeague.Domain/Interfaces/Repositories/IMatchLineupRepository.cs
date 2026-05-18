using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
    {
        Task<IEnumerable<MatchLineup>> GetByMatchIdAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);
        Task<MatchLineup?> GetByIdWithDetailsAsync(int id);
        Task<MatchLineup?> GetByMatchAndPlayerAsync(int matchId, int playerId);
        Task<int> CountStartersByMatchAndTeamAsync(int matchId, int teamId);
    }
}