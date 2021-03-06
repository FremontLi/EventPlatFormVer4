﻿using Microsoft.EntityFrameworkCore;


namespace EventPlatFormVer4.Models
{
    public class MvcEpfContext:DbContext
    {
        public MvcEpfContext(DbContextOptions<MvcEpfContext> options)
            : base(options)
        {
            //this.Database.EnsureCreated();
        }

        public DbSet<Participant> Participants { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }

    }
}
