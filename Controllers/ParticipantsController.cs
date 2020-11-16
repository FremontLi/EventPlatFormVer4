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
    public class ParticipantsController : Controller
    {
        private ParticipantService participantService;
        private EventParticipantService eventParticipantService;

        private readonly MvcEpfContext _context;

        public ParticipantsController(MvcEpfContext context)
        {
            _context = context;
            participantService = new ParticipantService(context);
        }


        // GET: participants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Participants.ToListAsync());
        }


        //参赛者主界面
        public async Task<IActionResult> Info(string id)
        {
            ViewData["Pid"] = id;
            return View(_context.Events.Where(item=>item.State==1));

        }

        //参赛者个人信息
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var participant = await participantService.Find(id);

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }
        //已报名的比赛
        public async Task<IActionResult> HaveApplied(string id)
        {
            ViewData["Pid"] = id;
            return View(_context.EventParticipants.Where(item => item.ParticipantId == id));
        }


        // GET: participants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: participants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,PassWd,Email,PhoneNum")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                await participantService.Add(participant);
                return RedirectToAction(nameof(Index));
            }
            return View(participant);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["participantId"] = id;
            var participant = await _context.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }
            return View(participant);
        }

        // POST: participants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,RoleID,Name,PassWd,Email,PhoneNum")] Participant participant)

        {
            if (id != participant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participant.ID))
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
            return View(participant);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await participantService.Find(id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await participantService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantExists(string id)

        {
            return _context.Participants.Any(e => e.ID == id);
        }

        //报名
        public async Task<IActionResult> Apply(string EventId,string id)
        {
            ViewData["Pid"] = id;
            ViewData["EventId"] = EventId;
            await participantService.Apply(EventId,id);
            _context.EventParticipants.UpdateRange();
            return View(_context.Events.Where(item => item.Id == EventId).FirstOrDefault());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply([Bind("ID,RoleID,Name,PassWd,Email,PhoneNum")] EventParticipant eventParticipant)
        {
            if (ModelState.IsValid)
            {
                await eventParticipantService.AddEP(eventParticipant);
                return RedirectToAction(nameof(Index));
            }
            return View(eventParticipant);
        }

        //退赛
        public async Task<IActionResult> ExitEvent(string id, [Bind("State")] EventParticipant EP)
        {
            ViewData["Pid"] = id;
            await participantService.ExitEvent(EP, id);
            return RedirectToAction(nameof(Info));
        }

    }
}
