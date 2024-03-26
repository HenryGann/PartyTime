using Microsoft.EntityFrameworkCore;
using PartyTime.Models;
using PartyTime.Contexts;
using System;
using System.Linq;

namespace PartyTime.Repository;

public class EventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EventDTO> GetEventWithUsername(int eventId)
    {
        var query = from e in _context.Events
                    where e.Id == eventId
                    join user in _context.Users on e.OwnerId equals user.Id
                    select new EventDTO
                    {
                        Id = e.Id,
                        Title = e.Title,
                        Summary = e.Summary,
                        Location = e.Location,
                        DateTime = e.DateTime,
                        EventCreator = user.Username
                        };

        return await query.FirstOrDefaultAsync();
    }
}
