﻿namespace DataGate.Services.Data.Entities
{
    using System;
    using System.Collections.Generic;

    using DataGate.Web.Dtos.Queries;

    public interface IEntityDetailsService
    {
        IAsyncEnumerable<string[]> GetByIdAndDate(string function, int id, DateTime? date);

        ContainerDto GetContainer(string function, int id, DateTime? date);

        IEnumerable<DistinctDocDto> GetDistinctDocuments(string function, int id, DateTime? date);

        IEnumerable<DistinctAgrDto> GetDistinctAgreements(string function, int id, DateTime? date);
    }
}