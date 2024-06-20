using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.BackendService
{
    public class ActionOutcome
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string>? Messages { get; set; } = new();
        public string Value { get; set; } = string.Empty;
    }
}
