using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.Account.Input
{
    public class ChangeStatusInputDto
    {
        public string UserId { get; set; }
        public int StatusId { get; set; }
    }
}
