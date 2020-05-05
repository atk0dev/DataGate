namespace DataGate.Services.Data.ViewSetups
{
    using System.Linq;

    using DataGate.Services.DateTime;
    using DataGate.Web.ViewModels.Entities;

    public class SpecificViewModelSetup
    {
        private const int IndexStartConnectionInSQLTable = 0;
        private const int IndexEndConnectionInSQLTable = 1;

        public static void PrepareModel(SpecificEntityViewModel model, IEntitySubEntitiesService service)
        {
            service.ThrowEntityNotFoundExceptionIfIdDoesNotExist(model.Id);

            var date = DateTimeParser.WebFormat(model.Date);
            int entityId = model.Id;

            model.Entity = service.GetEntityWithDateById(date, entityId).ToList();
            model.DistinctDocuments = service.GetDistinctDocuments(date, entityId).ToList();
            model.DistinctAgreements = service.GetDistinctAgreements(date, entityId).ToList();

            model.Values = service.GetEntity_SubEntities(date, entityId).ToList();
            model.Headers = service
                                                            .GetEntity_SubEntities(date, entityId)
                                                            .Take(1)
                                                            .FirstOrDefault()
                                                            .ToList();
            model.Timeline = service.GetTimeline(entityId).ToList();
            model.Documents = service.GetAllDocuments(entityId).ToList();
            model.Agreements = service.GetAllAgreements(date, entityId).ToList();

            string startConnection = model.Entity.ToList()[1][IndexStartConnectionInSQLTable];
            model.StartConnection = DateTimeParser.SqlFormat(startConnection);

            if (model.EndConnection != null)
            {
                string endConnection = model.Entity.ToList()[1][IndexEndConnectionInSQLTable];
                model.EndConnection = DateTimeParser.SqlFormat(endConnection);
            }

            //this.ViewData["DocumentFileTypes"] = this.fundsSelectListService.GetAllProspectusFileTypes();
            //this.ViewData["AgreementsFileTypes"] = this.fundsSelectListService.GetAllAgreementsFileTypes();
            //this.ViewData["AgreementsStatus"] = this.agreementsSelectListService.GetAllTbDomAgreementStatus();
            //this.ViewData["Companies"] = this.agreementsSelectListService.GetAllTbCompanies();
        }

        //private void CallEntitySubEntitiesWithSelectedColumns(SpecificEntityViewModel model, DateTime chosenDate)
        //{
        //    model.EntitySubEntities = this.service
        //        .GetFund_SubFundsWithSelectedViewAndDate(
        //                                        model.PreSelectedColumns,
        //                                        model.SelectedColumns,
        //                                        chosenDate,
        //                                        model.EntityId)
        //        .ToList();
        //}
    }
}
