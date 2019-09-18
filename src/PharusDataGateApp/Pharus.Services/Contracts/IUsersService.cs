﻿namespace Pharus.Services.Contracts
{
    using Pharus.Domain;

    using System.Collections.Generic;

    public interface IUsersService
    {
        List<PharusUser> GetAllUsers();

        PharusUser GetUser(string username);
    }
}