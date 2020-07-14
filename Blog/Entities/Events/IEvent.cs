using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Entities.Events
{
    public interface IEvent
    {
        DateTime Time { get; set; }
    }
}
