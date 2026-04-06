using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Text.RegularExpressions;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ITournamentRepository tournamentRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _tournamentRepository = tournamentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all sponsors");
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);
            var sponsor = await _sponsorRepository.GetByIdAsync(id);

            if (sponsor == null)
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);

            return sponsor;
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            ValidateEmail(sponsor.ContactEmail);

            if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name))
            {
                _logger.LogWarning("Sponsor with name '{Name}' already exists", sponsor.Name);
                throw new InvalidOperationException(
                    $"Ya existe un patrocinador con el nombre '{sponsor.Name}'.");
            }

            _logger.LogInformation("Creating sponsor: {Name}", sponsor.Name);
            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _sponsorRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {id}.");
            }

            ValidateEmail(sponsor.ContactEmail);

            if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name, id))
            {
                _logger.LogWarning(
                    "Sponsor name '{Name}' already taken by another sponsor", sponsor.Name);
                throw new InvalidOperationException(
                    $"Ya existe un patrocinador con el nombre '{sponsor.Name}'.");
            }

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;
            existing.Category = sponsor.Category;

            _logger.LogInformation("Updating sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _sponsorRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {id}.");
            }

            _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.DeleteAsync(id);
        }

        // ── TournamentSponsor ──

        public async Task<TournamentSponsor> LinkToTournamentAsync(
            int sponsorId, int tournamentId, decimal contractAmount)
        {
            // Validar que el sponsor existe
            var sponsorExists = await _sponsorRepository.ExistsAsync(sponsorId);
            if (!sponsorExists)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", sponsorId);
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {sponsorId}.");
            }

            // Validar que el torneo existe
            var tournamentExists = await _tournamentRepository.ExistsAsync(tournamentId);
            if (!tournamentExists)
            {
                _logger.LogWarning("Tournament with ID {TournamentId} not found", tournamentId);
                throw new KeyNotFoundException(
                    $"No se encontró el torneo con ID {tournamentId}.");
            }

            // Validar que no esté duplicada la vinculación
            if (await _tournamentSponsorRepository.ExistsAsync(tournamentId, sponsorId))
            {
                _logger.LogWarning(
                    "Sponsor {SponsorId} is already linked to tournament {TournamentId}",
                    sponsorId, tournamentId);
                throw new InvalidOperationException(
                    $"El patrocinador con ID {sponsorId} ya está vinculado al torneo con ID {tournamentId}.");
            }

            // Validar ContractAmount > 0
            if (contractAmount <= 0)
            {
                _logger.LogWarning(
                    "Invalid ContractAmount {Amount} for sponsor {SponsorId}", contractAmount, sponsorId);
                throw new InvalidOperationException(
                    "El monto del contrato debe ser mayor a 0.");
            }

            var tournamentSponsor = new TournamentSponsor
            {
                TournamentId = tournamentId,
                SponsorId = sponsorId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "Linking sponsor {SponsorId} to tournament {TournamentId}", sponsorId, tournamentId);
            return await _tournamentSponsorRepository.CreateAsync(tournamentSponsor);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            var exists = await _sponsorRepository.ExistsAsync(sponsorId);
            if (!exists)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", sponsorId);
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {sponsorId}.");
            }

            _logger.LogInformation("Retrieving tournaments for sponsor ID: {SponsorId}", sponsorId);
            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }

        public async Task UnlinkFromTournamentAsync(int sponsorId, int tournamentId)
        {
            var link = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

            if (link == null)
            {
                _logger.LogWarning(
                    "No link found between sponsor {SponsorId} and tournament {TournamentId}",
                    sponsorId, tournamentId);
                throw new KeyNotFoundException(
                    $"No existe vinculación entre el patrocinador con ID {sponsorId} y el torneo con ID {tournamentId}.");
            }

            _logger.LogInformation(
                "Unlinking sponsor {SponsorId} from tournament {TournamentId}", sponsorId, tournamentId);
            await _tournamentSponsorRepository.DeleteAsync(link.Id);
        }

        // ── Helpers ──

        private static void ValidateEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(email))
                throw new InvalidOperationException(
                    $"El email '{email}' no tiene un formato válido.");
        }
    }
}