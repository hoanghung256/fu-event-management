﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.UserUseCases
{
    public interface ILogin
    {
        Task<object> LoginAsync(string email, string password);
    }
}
