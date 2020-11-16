using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using EventPlatFormVer4.Data;
using EventPlatFormVer4.Models;
using EventPlatFormVer4.Service;

namespace EventPlatFormVer4.Controllers
{
    public class EventsController : Controller
    {
        private EventService eventService;
        private EventParticipantService eventParticipantService;
        private readonly MvcEpfContext _context;

        public EventsController(MvcEpfContext context)
        {
            _context = context;
            eventService = new EventService(context);
        }

       
        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // TODO: add a view
        // GET: Events/GetE-Ps/5 显示Event的所有Participants
        public async Task<IActionResult> GetEventParticipants(string EventId)
        {
            if(EventId == null)
            {
                return NotFound();
            }

            ViewData["EventId"] = EventId;
            return View(await eventService.GetEventParticipantsAsync(EventId));

        }
        // GET: Events/GetE-Ps/5 显示Event的所有Participants
        public async Task<IActionResult> GetParticipantEvents(string participantId)
        {
            ViewData["Participant_Id"] = participantId;
            //   return View(await eventParticipantService.GetParticipantEventsAsync(participantId));
            var ep = await _context.EventParticipants.Where(item => item.ParticipantId == participantId).ToListAsync();
            return View(ep);
        }
        // GET: Events/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        public async Task<IActionResult> DetailsForParticipants(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }
        // GET: Events/Create: 创建新Event
        public IActionResult Create(string id)
        {
            ViewData["sid"] = id;
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id,[Bind("Name,SponsorId,Rank,EventStartTime,EventEndTime,SignUpStartTime,SignUpEndTime,Address,Detail")] Event @event)
        {
            if (ModelState.IsValid)
            {
                await eventService.AddEvent(@event);
                return RedirectToAction("Info_Apply","Sponsors",new { id=id});
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Rank,EventStartTime,EventEndTime,SignUpStartTime,SignUpEndTime,Address,Detail")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                    */
                    await eventService.UpdateEventParticipants(@event); // 更新 E-Participants
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            var @event = await eventService.FindEventAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string EventId)
        {
            /*
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            */
            await eventService.RemoveEventParticipants(EventId);
            await eventService.RemoveEvent(EventId);
            return RedirectToAction(nameof(Index));
        }


        private bool EventExists(string id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
