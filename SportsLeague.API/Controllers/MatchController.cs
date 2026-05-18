using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL base: /api/match
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IMatchLineupService _matchLineupService;
        private readonly IMapper _mapper;

        public MatchController(
            IMatchService matchService,
            IMatchLineupService matchLineupService,
            IMapper mapper)
        {
            _matchService = matchService;
            _matchLineupService = matchLineupService;
            _mapper = mapper;
        }

        [HttpGet("tournament/{tournamentId}")] // URL: /api/match/tournament/{tournamentId}
        public async Task<ActionResult<IEnumerable<MatchResponseDTO>>> GetByTournament(int tournamentId)
        {
            try
            {
                var matches = await _matchService.GetAllByTournamentAsync(tournamentId);
                return Ok(_mapper.Map<IEnumerable<MatchResponseDTO>>(matches));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchResponseDTO>> GetById(int id)
        {
            var match = await _matchService.GetByIdAsync(id);
            return Ok(_mapper.Map<MatchResponseDTO>(match));
        }

        [HttpPost]
        public async Task<ActionResult<MatchResponseDTO>> Create(MatchRequestDTO dto)
        {
            try
            {
                var match = _mapper.Map<Match>(dto);
                var created = await _matchService.CreateAsync(match);
                var matchWithDetails = await _matchService.GetByIdAsync(created.Id);
                var responseDto = _mapper.Map<MatchResponseDTO>(matchWithDetails);
                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MatchRequestDTO dto)
        {
            try
            {
                var match = _mapper.Map<Match>(dto);
                await _matchService.UpdateAsync(id, match);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _matchService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, UpdateMatchStatusDTO dto)
        {
            try
            {
                await _matchService.UpdateStatusAsync(id, dto.Status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Player line-up for the match
        [HttpPost("{matchId}/lineup")]
        public async Task<ActionResult<MatchLineupDTO>> AddToLineup(
           int matchId, CreateMatchLineupDTO dto)
        {
            try
            {
                var lineup = await _matchLineupService.AddPlayerAsync(
                    matchId, dto.PlayerId, dto.IsStarter, dto.Position);
                return Ok(_mapper.Map<MatchLineupDTO>(lineup));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpGet("{matchId}/lineup")]
        public async Task<ActionResult<IEnumerable<MatchLineupDTO>>> GetLineup(int matchId)
        {
            try
            {
                var lineup = await _matchLineupService.GetByMatchAsync(matchId);
                return Ok(_mapper.Map<IEnumerable<MatchLineupDTO>>(lineup));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

        [HttpGet("{matchId}/lineup/team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupDTO>>> GetLineupByTeam(
            int matchId, int teamId)
        {
            try
            {
                var lineup = await _matchLineupService.GetByMatchAndTeamAsync(matchId, teamId);
                return Ok(_mapper.Map<IEnumerable<MatchLineupDTO>>(lineup));
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{matchId}/lineup/{id}")]
        public async Task<ActionResult> DeleteFromLineup(int matchId, int id)
        {
            try
            {
                await _matchLineupService.DeleteAsync(matchId, id);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

    }
}