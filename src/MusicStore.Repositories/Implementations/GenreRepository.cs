using MusicStore.Entities;
using MusicStore.Persistence;
using MusicStore.Repositories.Abstractions;

namespace MusicStore.Repositories.Implementations
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
