using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Entities.Info;
using MusicStore.Persistence;
using MusicStore.Repositories.Abstractions;

namespace MusicStore.Repositories.Implementations
{
    public class ConcertRepository : RepositoryBase<Concert>, IConcertRepository
    {
        public ConcertRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<ICollection<Concert>> GetAsync()
        {
            // Eager loading approach
            return await _context.Set<Concert>()
                .Include(x => x.Genre)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<ConcertInfo>> GetAsync(string? title)
        {
            // Eager loading apooroach optimizado
            return await _context.Set<Concert>()
                .Include(x => x.Genre)
                .Where(x => x.Title.Contains(title ?? string.Empty))
                .AsNoTracking()
                .Select(x => new ConcertInfo
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Place = x.Place,
                    UnitPrice = x.UnitPrice,
                    Genre = x.Genre.Name,
                    GenreId = x.GenreId,
                    DateEvent = x.DateEvent.ToShortDateString(),
                    TimeEvent = x.DateEvent.ToShortTimeString(),
                    ImageUrl = x.ImageUrl,
                    TicketsQuantity = x.TicketsQuantity,
                    Finalized = x.Finalized,
                    Status = x.Status ? "Activo" : "Inactivo"
                })
                .ToListAsync();
        }
    }
}
