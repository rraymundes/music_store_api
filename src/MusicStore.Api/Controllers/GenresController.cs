using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories.Abstractions;
using System.Net;

namespace MusicStore.Api.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _repository;
        private readonly ILogger<GenresController> _logger;

        public GenresController(IGenreRepository repository,
            ILogger<GenresController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var response = new BaseResponseGeneric<ICollection<GenreResponseDto>>();

            try
            {
                response.Data = await _repository.GetAsync();
                response.Success = true;
                _logger.LogInformation("Obteniendo todos los generos musicales");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id) 
        {
            var response = new BaseResponseGeneric<GenreResponseDto>();

            try
            {
                response.Data = await _repository.GetAsync(id);
                response.Success = true;
                _logger.LogInformation($"Obteniendo el genero musical con el id: {id}.");
                return response.Data is not null ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(GenreRequestDto genreRequestDto)
        { 
            var response = new BaseResponseGeneric<int>();

            try
            {
                var genreId = await _repository.AddAsync(genreRequestDto);
                response.Success = true;
                response.Data = genreId;
                _logger.LogInformation($"Género musical insertado con id: {genreId}");
                return StatusCode((int)HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al insertar.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, GenreRequestDto genreRequestDto)
        {
            var response = new BaseResponse();

            try
            {
                await _repository.UpdateAsync(id, genreRequestDto);
                response.Success = true;
                _logger.LogInformation($"Género musical con id: {id} actualizado.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al actualizar.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse();

            try
            {
                await _repository.DeleteAsync(id);
                response.Success = true;
                _logger.LogInformation($"Género musical con id: {id} eliminado.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al eliminar.";
                _logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
                return BadRequest(response);
            }
        }
    }
}
