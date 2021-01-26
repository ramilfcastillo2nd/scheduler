﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_core.api.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
