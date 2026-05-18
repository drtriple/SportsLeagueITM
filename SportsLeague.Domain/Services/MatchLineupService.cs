using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Helper;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _matchLineupRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly MatchValidationHelper _matchValidationHelper;
        private readonly ILogger<MatchLineupService> _logger;

        public MatchLineupService(
            IMatchLineupRepository matchLineupRepository,
            IMatchRepository matchRepository,
            MatchValidationHelper matchValidationHelper,
            ILogger<MatchLineupService> logger)
        {
            _matchLineupRepository = matchLineupRepository;
            _matchRepository = matchRepository;
            _matchValidationHelper = matchValidationHelper;
            _logger = logger;
        }

        public async Task<MatchLineup> AddPlayerAsync(
            int matchId, int playerId, bool isStarter, string position)
        {
            // Valida: partido existe + estado Scheduled
            var match = await _matchValidationHelper.ValidateMatchForLineupAsync(matchId);

            // Valida: jugador existe + pertenece a uno de los equipos del partido
            var player = await _matchValidationHelper.ValidatePlayerInMatchAsync(playerId, match);

            // Validar que el jugador no esté registrado dos veces en la misma alineación
            var existing = await _matchLineupRepository.GetByMatchAndPlayerAsync(matchId, playerId);
            if (existing != null)
            {
                _logger.LogWarning(
                    "Player {PlayerId} is already registered in lineup for match {MatchId}",
                    playerId, matchId);
                throw new InvalidOperationException(
                    "El jugador ya está registrado en la alineación de este partido");
            }

            // Validar máximo 11 titulares por equipo
            if (isStarter)
            {
                var starterCount = await _matchLineupRepository
                    .CountStartersByMatchAndTeamAsync(matchId, player.TeamId);

                if (starterCount >= 11)
                {
                    _logger.LogWarning(
                        "Team {TeamId} already has 11 starters in match {MatchId}",
                        player.TeamId, matchId);
                    throw new InvalidOperationException(
                        "El equipo ya tiene 11 titulares registrados en este partido");
                }
            }

            var lineup = new MatchLineup
            {
                MatchId = matchId,
                PlayerId = playerId,
                IsStarter = isStarter,
                Position = position,
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "Adding player {PlayerId} to lineup of match {MatchId}", playerId, matchId);
            var created = await _matchLineupRepository.CreateAsync(lineup);
            return (await _matchLineupRepository.GetByIdWithDetailsAsync(created.Id))!;
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
        {
            var matchExists = await _matchRepository.ExistsAsync(matchId);
            if (!matchExists)
            {
                _logger.LogWarning("Match with ID {MatchId} not found", matchId);
                throw new KeyNotFoundException(
                    $"No se encontró el partido con ID {matchId}");
            }

            var lineup = await _matchLineupRepository.GetByMatchIdAsync(matchId);

            if (!lineup.Any())
            {
                _logger.LogWarning("Match {MatchId} has no lineup registered", matchId);
                throw new InvalidOperationException(
                    $"El partido con ID {matchId} no tiene alineación registrada");
            }

            _logger.LogInformation("Retrieving lineup for match ID: {MatchId}", matchId);
            return lineup;
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
            {
                _logger.LogWarning("Match with ID {MatchId} not found", matchId);
                throw new KeyNotFoundException(
                    $"No se encontró el partido con ID {matchId}");
            }

            if (match.HomeTeamId != teamId && match.AwayTeamId != teamId)
            {
                _logger.LogWarning(
                    "Team {TeamId} is not part of match {MatchId}", teamId, matchId);
                throw new InvalidOperationException(
                    "El equipo no participa en este partido");
            }

            var lineup = await _matchLineupRepository.GetByMatchAndTeamAsync(matchId, teamId);

            if (!lineup.Any())
            {
                _logger.LogWarning(
                    "Team {TeamId} has no lineup registered for match {MatchId}", teamId, matchId);
                throw new InvalidOperationException(
                    $"El equipo con ID {teamId} no tiene alineación registrada en el partido con ID {matchId}");
            }

            _logger.LogInformation(
                "Retrieving lineup for match {MatchId}, team {TeamId}", matchId, teamId);
            return lineup;
        }

        public async Task DeleteAsync(int matchId, int lineupId)
        {
            var lineup = await _matchLineupRepository.GetByIdAsync(lineupId);
            if (lineup == null || lineup.MatchId != matchId)
            {
                _logger.LogWarning(
                    "Lineup entry {LineupId} not found for match {MatchId}", lineupId, matchId);
                throw new KeyNotFoundException(
                    $"No se encontró el registro de alineación con ID {lineupId} para el partido con ID {matchId}");
            }

            _logger.LogInformation(
                "Deleting lineup entry {LineupId} from match {MatchId}", lineupId, matchId);
            await _matchLineupRepository.DeleteAsync(lineupId);
        }
    }
}