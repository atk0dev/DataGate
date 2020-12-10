﻿namespace DataGate.Web.ViewModels.CountriesDist
{
    using DataGate.Services.Mapping;
    using DataGate.Web.Dtos.Queries;

    public class CountryDistViewModel : IMapFrom<CountryDistDto>
    {
        public int Id { get; set; }

        public string IsoCountry { get; set; }

        public string LocalRepresentative { get; set; }

        public string PayingAgent { get; set; }

        public string LegalSupport { get; set; }

        public string Language { get; set; }
    }
}
