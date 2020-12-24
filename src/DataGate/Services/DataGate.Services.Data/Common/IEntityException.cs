﻿// Copyright (c) DataGate Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DataGate.Services.Data.Common
{
    public interface IEntityException
    {
        bool DoesEntityExist(int id);
    }
}
