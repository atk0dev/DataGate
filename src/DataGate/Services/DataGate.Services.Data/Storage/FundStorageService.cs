﻿// Copyright (c) DataGate Project. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DataGate.Services.Data.Storage
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using DataGate.Common.Exceptions;
    using DataGate.Data.Common.Repositories;
    using DataGate.Data.Common.Repositories.AppContext;
    using DataGate.Data.Models.Entities;
    using DataGate.Services.Data.Storage.Contracts;
    using DataGate.Services.Mapping;
    using DataGate.Services.SqlClient;
    using DataGate.Services.SqlClient.Contracts;
    using DataGate.Web.Dtos.Entities;
    using DataGate.Web.Infrastructure.Extensions;
    using DataGate.Web.InputModels.Funds;

    public class FundStorageService : IFundStorageService
    {
        private readonly string sqlFunctionId = "[fn_fund_id]";

        private readonly ISqlQueryManager sqlManager;
        private readonly IAppRepository<TbHistoryFund> repository;
        private readonly IFundSelectListService service;

        public FundStorageService(
                        ISqlQueryManager sqlQueryManager,
                        IAppRepository<TbHistoryFund> repository,
                        IFundSelectListService service)
        {
            this.sqlManager = sqlQueryManager;
            this.repository = repository;
            this.service = service;
        }

        public T ByIdAndDate<T>(int id, string date)
        {
            this.ThrowEntityNotFoundExceptionIfIdDoesNotExist(id);

            var dateParsed = DateTimeParser.FromWebFormat(date);
            var dto = this.sqlManager
                .ExecuteQueryMapping<EditFundGetDto>(this.sqlFunctionId, id, dateParsed)
                .FirstOrDefault();

            var fundAdmin = dto.FACode.Contains("CAC") ?
                "CACEIS" : dto.FACode.Contains("ROT") ? 
                "Edmond de Rothschild" : "Northern Trust";

            dto.FundAdmin = fundAdmin; 

            return AutoMapperConfig.MapperInstance.Map<T>(dto);
        }

        public async Task<int> Create(CreateFundInputModel model)
        {
            FundPostDto dto = AutoMapperConfig.MapperInstance.Map<FundPostDto>(model);
            dto.EndDate = DateTimeParser.ToSqlFormat(model.EndDate);

            await this.SetForeignKeys(dto, model.Status, model.LegalForm, model.LegalVehicle,
                                      model.LegalType, model.CompanyTypeDesc);

            SqlCommand command = this.AssignBaseParameters(dto, SqlProcedureDictionary.CreateFund);

            // Assign particular parameters
            command.Parameters.Add(new SqlParameter("@f_endDate", SqlDbType.NVarChar) { Value = dto.EndDate });

            await this.sqlManager.ExecuteProcedure(command);

            var fundId = this.repository.All()
                .Where(f => f.FOfficialFundName == dto.FundName)
                .Select(f => f.FId)
                .FirstOrDefault();

            return fundId;
        }

        public async Task<int> Edit(EditFundInputModel model)
        {
            FundPostDto dto = AutoMapperConfig.MapperInstance.Map<FundPostDto>(model);

            await this.SetForeignKeys(dto, model.Status, model.LegalForm, model.LegalVehicle,
                                      model.LegalType, model.CompanyTypeDesc);

            SqlCommand command = this.AssignBaseParameters(dto, SqlProcedureDictionary.EditFund);

            // Assign particular parameters
            command.Parameters.AddRange(new[]
                   {
                            new SqlParameter("@f_id", SqlDbType.Int) { Value = dto.Id },
                            new SqlParameter("@comment", SqlDbType.NVarChar) { Value = dto.CommentArea },
                            new SqlParameter("@comment_title ", SqlDbType.NVarChar) { Value = dto.CommentTitle },
                            new SqlParameter("@f_shortFundName", SqlDbType.NVarChar) { Value = dto.FundName },
                   });

            await this.sqlManager.ExecuteProcedure(command);

            return dto.Id;
        }

        public async Task<bool> DoesExist(string name)
        {
            return await this.repository.All().AnyAsync(f => f.FOfficialFundName == name);
        }

        public async Task<bool> DoesExistAtDate(string name, DateTime initialDate)
        {
            return await this.repository.All().AnyAsync(f => f.FOfficialFundName == name && f.FInitialDate == initialDate);
        }

        private async Task SetForeignKeys(FundPostDto dto, string status,
                                          string legalform, string legalVehicle,
                                          string legalType, string companyType)
        {
            dto.Status = await this.service.ByIdStatus(status);
            dto.LegalForm = await this.service.ByIdLegalForm(legalform);
            dto.LegalVehicle = await this.service.ByIdLegalVehicle(legalVehicle);
            dto.LegalType = await this.service.ByIdLegalType(legalType);
            dto.CompanyTypeDesc = await this.service.ByIdCompanyType(companyType);
        }

        private SqlCommand AssignBaseParameters(FundPostDto dto, string procedure)
        {
            SqlCommand command = new SqlCommand(procedure);

            command.Parameters.AddRange(new[]
                   {
                            new SqlParameter("@f_initialDate", SqlDbType.NVarChar) { Value = dto.InitialDate },
                            new SqlParameter("@f_status", SqlDbType.Int) { Value = dto.Status },
                            new SqlParameter("@f_registrationNumber", SqlDbType.NVarChar) { Value = dto.RegNumber },
                            new SqlParameter("@f_officialFundName", SqlDbType.NVarChar) { Value = dto.FundName },
                            new SqlParameter("@f_leiCode", SqlDbType.NVarChar) { Value = dto.LEICode },
                            new SqlParameter("@f_cssfCode", SqlDbType.NVarChar) { Value = dto.CSSFCode },
                            new SqlParameter("@f_faCode", SqlDbType.NVarChar) { Value = dto.FACode },
                            new SqlParameter("@f_depCode", SqlDbType.NVarChar) { Value = dto.DEPCode },
                            new SqlParameter("@f_taCode", SqlDbType.NVarChar) { Value = dto.TACode },
                            new SqlParameter("@f_legalForm", SqlDbType.Int) { Value = dto.LegalForm },
                            new SqlParameter("@f_legalType", SqlDbType.Int) { Value = dto.LegalType },
                            new SqlParameter("@f_legal_vehicle", SqlDbType.Int) { Value = dto.LegalVehicle },
                            new SqlParameter("@f_companyType", SqlDbType.Int) { Value = dto.CompanyTypeDesc },
                            new SqlParameter("@f_tinNumber", SqlDbType.NVarChar) { Value = dto.TinNumber },
                            new SqlParameter("@f_VATRegistrationNumber", SqlDbType.NVarChar) { Value = dto.VATRegNumber },
                            new SqlParameter("@f_VATIdentificationNumber", SqlDbType.NVarChar) { Value = dto.VATIdentificationNumber },
                            new SqlParameter("@f_IBICNumber", SqlDbType.NVarChar) { Value = dto.IBICNumber },
                   });
            return command;
        }

        private void ThrowEntityNotFoundExceptionIfIdDoesNotExist(int id)
        {
            if (!this.Exists(id))
            {
                throw new EntityNotFoundException(nameof(TbHistoryFund));
            }
        }

        private bool Exists(int id) => this.repository.All().Any(x => x.FId == id);
    }
}
