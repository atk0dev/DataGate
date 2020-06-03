﻿namespace DataGate.Web.Areas.SubFunds.Controllers
{
    using System.Threading.Tasks;

    using DataGate.Common;
    using DataGate.Services.Data.Entities;
    using DataGate.Services.Data.SubFunds;
    using DataGate.Services.Data.ViewSetups;
    using DataGate.Web.Controllers;
    using DataGate.Web.Dtos.Queries;
    using DataGate.Web.Helpers;
    using DataGate.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.SubFundAreaName)]
    [Authorize]
    public class SubFundDetailsController : BaseController
    {
        private readonly IEntityDetailsService service;
        private readonly ISubFundService subFundService;

        public SubFundDetailsController(
                                    IEntityDetailsService service,
                                    ISubFundService subFundService)
        {
            this.service = service;
            this.subFundService = subFundService;
        }

        [ActionName("Details")]
        [Route("sf/{id}/{date}")]
        public async Task<IActionResult> ByIdAndDate(int id, string date)
        {
            var dto = new QueriesToPassDto()
            {
                SqlFunctionById = QueryDictionary.SqlFunctionByIdSubFund,
                SqlFunctionDistinctDocuments = QueryDictionary.SqlFunctionDistinctDocumentsSubFund,
                SqlFunctionDistinctAgreements = QueryDictionary.SqlFunctionDistinctAgreementsSubFund,
                SqlFunctionContainer = QueryDictionary.SqlFunctionContainerFund,
            };

            var viewModel = await SpecificVMSetup.SetGet<SpecificEntityViewModel>(id, date, this.service, this.subFundService, dto);
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Update([Bind("Command,Date,Id")] SpecificEntityViewModel viewModel)
        {
            if (viewModel.Command == GlobalConstants.CommandUpdateTable)
            {
                return this.ShowInfo(InfoMessages.SuccessfulUpdate, GlobalConstants.SubFundDetailsRouteName, new { viewModel.Id, viewModel.Date });
            }

            return this.ShowError(ErrorMessages.UnsuccessfulUpdate, GlobalConstants.SubFundDetailsRouteName, new { viewModel.Id, viewModel.Date });
        }
    }
}