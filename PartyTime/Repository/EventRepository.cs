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

    public async Task<Event> GetEvent(int eventId)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
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

    public async Task<List<EventDTO>> GetAllEvents()
    {
        var query = from e in _context.Events
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

        return await query.ToListAsync();
    }

    public async Task<Event> NewEventDTOtoEvent(NewEventDTO eventDTO)
    {
        if (eventDTO == null)
        {
            throw new ArgumentNullException(nameof(eventDTO));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == eventDTO.EventCreator);

        if (user == null)
        {
            throw new ArgumentException($"Event with owner username '{eventDTO.EventCreator}' not found");
        }

        var newEvent = new Event(eventDTO, user.Id);

        return newEvent;
    }

    public async Task<Event> EventDTOtoEvent(EventDTO eventDTO)
    {
        if (eventDTO == null)
        {
            throw new ArgumentNullException(nameof(eventDTO));
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == eventDTO.EventCreator);

        if (user == null)
        {
            throw new ArgumentException($"Event with owner username '{eventDTO.EventCreator}' not found");
        }

        var newEvent = new Event(eventDTO, user.Id);

        return newEvent;
    }
}
