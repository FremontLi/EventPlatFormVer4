﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlatFormVer4.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EventPlatFormVer4.Service
{
    public class SponsorService
    {
        
        private static MvcEpfContext _context;
        public SponsorService(MvcEpfContext context)
        {
            _context = context;
        }

        public void Add(Sponsor sponsor)//新增赞助者
        {
            try
            {
                using (var db = _context)
                {
                    db.Sponsors.Add(sponsor);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("添加失败");
            }
        }
        public void Delete(string id)//删除赞助者
        {
            using (var db = _context)
            {
                var sponsor = db.Sponsors.Where(item => item.Id == id);
                db.Sponsors.RemoveRange(sponsor);
                db.SaveChanges();
            }
        }
        public void Update(Sponsor sponsor)//更新赞助者
        {
            using (var db = _context)
            {
                db.Update(sponsor);
                db.SaveChanges();
            }
        }
        public Sponsor Find(string id)//查找赞助者
        {
            using (var db = _context)
            {
                var sponsor = db.Sponsors.Where(item => item.Id == id);
                return (Sponsor)sponsor;
            }
        }

        // -----------申请举办event,只要申请了，活动就写入到数据库，event的State1=0为待审核
        public void Apply(string id)
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Insert;
                @event.State = 0;//Todo，改为State1
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }

        // -----------取消event,event的State1修改为2，（State1=1为管理员审核通过）
        public void Cancel(string id)
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                @event.State = 2;//Todo，改为State1
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }

        // -----------审核participant报名
        public void Accept(string id)//同意,将event的State2修改为1
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                @event.State = 1;//Todo，改为State2
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }
        public void Deny(string id)//拒绝，将event的State2修改为2
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                @event.State = 2;//Todo，改为State2
                db.Events.Update(@event);
                db.SaveChanges();
            }
        }
        
        public Event Verify(string id)//检查，将所以未审核的全部展示出来
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                return @event;
            }
        }

        public Event Alter(string id)//修改participant申请表的State2
        {
            using (var db = _context)
            {
                Event @event = (Event)db.Events.Where(item => item.Id == id);
                return @event;
            }
        }

        // -----------登记participant成绩


        // -----------显示自己已经申请的event列表
        public List<Event> ApplyEvents(int state)
        {
            using (var db = _context)
            {
                var queue = db.Events.Where(item => item.State == state);
                return queue.ToList();//打印event列表
            }
        }
    }
}
