using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
    {
        public MatchLineupRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchIdAsync(int matchId)
        {
            return await _dbSet
                .Where(ml => ml.MatchId == matchId)
                .Include(ml => ml.Player)
                    .ThenInclude(p => p.Team)
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
        {
            return await _dbSet
                .Where(ml => ml.MatchId == matchId && ml.Player.TeamId == teamId)
                .Include(ml => ml.Player)
                    .ThenInclude(p => p.Team)
                .ToListAsync();
        }

        public async Task<MatchLineup?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(ml => ml.Id == id)
                .Include(ml => ml.Player)
                    .ThenInclude(p => p.Team)
                .FirstOrDefaultAsync();
        }

        public async Task<MatchLineup?> GetByMatchAndPlayerAsync(int matchId, int playerId)
        {
            return await _dbSet
                .Where(ml => ml.MatchId == matchId && ml.PlayerId == playerId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountStartersByMatchAndTeamAsync(int matchId, int teamId)
        {
            return await _dbSet
                .Where(ml => ml.MatchId == matchId
                          && ml.IsStarter
                          && ml.Player.TeamId == teamId)
                .CountAsync();
        }
    }
}