using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories.Abstractions;

namespace MusicStore.Api.Controllers
{
    [ApiController]
    [Route("api/concerts")]
    public class ConcertsController : ControllerBase
    {
        private readonly IConcertRepository _repository;
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<ConcertsController> _logger;

        public ConcertsController(
            IConcertRepository repository,
            IGenreRepository genreRepository,
            ILogger<ConcertsController> logger)
        {
            _repository = repository;
            _genreRepository = genreRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var concertsDb = await _repository.GetAsync();
            return Ok(concertsDb);
        }

        [HttpGet("title")]
        public async Task<IActionResult> Get(string? title)
        {
            /*var response = new BaseResponseGeneric<ICollection<ConcertResponseDto>>();

            try
            {
                var concertsDb = await _repository.GetAsync(x => x.Title.Contains(title ?? string.Empty), x => x.DateEvent);
                
                var concertsDto = concertsDb.Select(x => new ConcertResponseDto
                {
                    Title = x.Title,
                    Description = x.Description,
                    Place = x.Place,
                    UnitPrice = x.UnitPrice,
                    GenreId = x.GenreId,
                    DateEvent = x.DateEvent,
                    ImageUrl = x.ImageUrl,
                    TicketsQuantity = x.TicketsQuantity,
                    Finalized = x.Finalized
                }).ToList();

                response.Data = concertsDto;
                response.Success = true;
                _logger.LogInformation("Obteniendo todos los conciertos");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al obtener la información.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }*/

            var concerts = await _repository.GetAsync(title);
            return Ok(concerts);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ConcertRequestDto concertRequestDto)
        {
            var response = new BaseResponseGeneric<int>();

            try
            {
                var genre = await _genreRepository.GetAsync(concertRequestDto.GenreId);

                if(genre is null)
                {
                    response.ErrorMessage = $"El id del genero {concertRequestDto.GenreId} es incorrecto.";
                    _logger.LogWarning(response.ErrorMessage);
                    return BadRequest(response);
                }

                var concertDb = new Concert
                {
                    Title = concertRequestDto.Title,
                    Description = concertRequestDto.Description,
                    Place = concertRequestDto.Place,
                    UnitPrice = concertRequestDto.UnitPrice,
                    GenreId = concertRequestDto.GenreId,
                    DateEvent = concertRequestDto.DateEvent,
                    ImageUrl = concertRequestDto.ImageUrl,
                    TicketsQuantity = concertRequestDto.TicketsQuantity
                };

                response.Data = await _repository.AddAsync(concertDb);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrio un error al guardar la informacion.";
                _logger.LogError(ex, ex.Message);
            }

            return Ok(response);
        }
    }
}
