using Microsoft.AspNetCore.Mvc;
using MusicStore.Entities;
using MusicStore.Repositories;

namespace MusicStore.Api.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly GenreRepository _repository;

        public GenresController(GenreRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<Genre>> Get() 
        {
            var data = _repository.Get();
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Genre> Get(int id) 
        {
            var item = _repository.Get(id);
            return item is not null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public ActionResult<Genre> Pos(Genre genre)
        { 
            _repository.Add(genre);
            return Ok(genre);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Genre genre)
        {
            _repository.Update(id, genre);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
    }
}
