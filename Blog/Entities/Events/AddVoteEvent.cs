﻿using Blog.Entities.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entities.Events
{
    public class AddVoteEvent : IEvent
    {
        public int VoteId { get; set; }
        public DateTime Time { get; set; }
    }
}
