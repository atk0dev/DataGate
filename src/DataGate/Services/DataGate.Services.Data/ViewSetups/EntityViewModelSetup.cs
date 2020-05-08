﻿// Service class for setting up
// view model properties

// Created: 04/2020
// Author:  Philip Shishov

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
namespace DataGate.Services.Data.ViewSetups
{
    using System.Linq;

    using DataGate.Services.DateTime;
    using DataGate.Services.Mapping;
    using DataGate.Web.ViewModels.Entities;
    using DataGate.Web.ViewModels.Queries;

    public static class EntityViewModelSetup
    {
        public static void SetPostProperties(EntitiesOverviewViewModel model, IEntityService service)
        {
            // ---------------------------------------------------------
            //
            // Available header column selection
            var headers = service.GetHeaders().ToList();
            model.HeadersSelection = headers;

            bool isInSelectionMode = model.SelectedColumns != null ? true : false;

            var date = DateTimeParser.WebFormat(model.Date);

            // Algorithm for getting values based on:
            // 0. Date update of table
            // 1. Selection mode as columns or not
            // 2. Active entities or not
            // 3. Selected entity
            if (isInSelectionMode)
            {
                var dto = AutoMapperConfig.MapperInstance.Map<GetWithSelectionDto>(model);

                if (model.IsActive)
                {
                    CallAllActiveWithSelectedColumns(model, dto, service);
                }
                else if (!model.IsActive)
                {
                    CallAllWithSelectedColumns(model, dto, service);
                }
            }
            else if (!isInSelectionMode)
            {
                if (model.IsActive)
                {
                    model.Values = service.GetAllActive(date, null, 1).ToList();
                }
                else if (!model.IsActive)
                {
                    model.Values = service.GetAll(date, null, 1).ToList();
                }
            }

            if (model.SelectTerm != null)
            {
                model.Values = CreateTableView.AddTableToView(model.Values, model.SelectTerm.ToLower());
            }
        }

        private static void CallAllWithSelectedColumns(EntitiesOverviewViewModel model, GetWithSelectionDto dto, IEntityService service)
        {
            model.Values = service.GetAllSelected(dto, null, 1).ToList();

            model.Headers = service.GetAllSelected(dto, 1).FirstOrDefault().ToList();
        }

        private static void CallAllActiveWithSelectedColumns(EntitiesOverviewViewModel model, GetWithSelectionDto dto, IEntityService service)
        {
            model.Values = service.GetAllActiveSelected(dto, null, 1).ToList();

            model.Headers = service.GetAllActiveSelected(dto, 1).FirstOrDefault().ToList();
        }
    }
}