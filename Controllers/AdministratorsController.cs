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
    public class AdministratorsController : Controller
    {

        private AdministratorService administratorService;

        private readonly MvcEpfContext _context;

        public AdministratorsController(MvcEpfContext context)
        {
            _context = context;
            administratorService = new AdministratorService(context);
        }

        // GET: Administrators
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administrators.ToListAsync());
        }
        public async Task<IActionResult> Info(string id)
        {
            ViewData["admid"] = id;
            return View(await _context.Events.ToListAsync());
        }

        public async Task<IActionResult> Loading(string name ,string pwd,string role)
        {
            if (role == "0")
            {
                string s = await administratorService.loading(name, pwd);
                return RedirectToAction("Details", "Administrators", new { id = s });
            }
            else if (role=="1")
            {
                var sponsor = await _context.Sponsors.Where(item => item.Name == name && item.Pwd == pwd).FirstOrDefaultAsync();
                string s;
                if (sponsor == null) s = null;
                else s = sponsor.Id;
                return RedirectToAction("Details", "Sponsors", new { id = s });
            }
            else
            {
                var participant = await _context.Participants.Where(item => item.Name == name && item.PassWd == pwd).FirstOrDefaultAsync();
                string s;
                if (participant == null) s = null;
                else s = participant.ID;
                return RedirectToAction("Details", "Participants", new { id = s });
            }
        }

        // GET: Administrators/Details/5
        public async Task<IActionResult> Details(string? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index","Home");
            }
            var administrator = await administratorService.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        //GET:Administrators/Verify:审核
        [HttpGet]
        public async Task<IActionResult> Verify(string EventId,string admid)
        {
            ViewData["aid"] = admid;
           return View(await administratorService.EventInformation(EventId));
        }

        //:Administrators/Accept：接受未审核
        public async Task<IActionResult> Accept(string id, [Bind("State")] Event events)
        {
            await administratorService.Accept(id);
            return RedirectToAction(nameof(Info));
        }

        //Get:Administrator/Ban:禁止未审核
        public async Task<IActionResult>Ban(string id ,[Bind("State")] Event events)
        {
            await administratorService.Deny(id);
            return RedirectToAction(nameof(Info));
        }

        public IActionResult Login()
        {
            return View();
        }

        // GET: Administrators/Create:创建管理者
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string role,[Bind("Id,RoleID,Name,Email,Phone,Pwd")] Administrator administrator)
        {
            if (role == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (role == "0")
            {
                if (ModelState.IsValid)
                {
                    await administratorService.Add(administrator);
                    return RedirectToAction(nameof(Index));
                }
                return View(administrator);
            }
            else if (role == "1")
            {
                return RedirectToAction("Create", "Sponsors");
            }
            else
            {
                return RedirectToAction("Create", "Participants");
            }
            
        }

        // GET: Administrators/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator == null)
            {
                return NotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,RoleID,Name,Email,Phone,Pwd")] Administrator administrator)
        {
            if (id != administrator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                await administratorService.Update(administrator);
                return RedirectToAction("Details","Administrators",new { id = id});
            }
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await administratorService.FindAsync(id);
            ViewData["aid"] = administrator.Id;
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await administratorService.Delete(id);
            return RedirectToAction("Index","Home");
        }
    }
}
