﻿namespace Pharus.Services.Funds
{
    using System;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Pharus.Data;
    using Pharus.Services.Contracts;
    using Pharus.Services.Utilities;

    public class FundsService : IFundsService
    {
        private readonly string defaultDate = DateTime.Today.ToString("yyyyMMdd");
        private readonly string ConnectionString = DbConfiguration.ConnectionStringPharusProd.ToString();

        public List<string[]> GetAllActiveFunds()
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from fn_active_fund('{defaultDate}')";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetAllActiveFunds(DateTime? chosenDate)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from fn_active_fund('{defaultDate}')";
                }

                else
                {
                    command.CommandText = $"select * from fn_active_fund('{chosenDate?.ToString("yyyyMMdd")}')";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetActiveFundById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from fn_active_fund('{defaultDate}') where [FUND ID PHARUS] = {Id}";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetActiveFundById(DateTime? chosenDate, int Id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (chosenDate == null)
                {
                    command.CommandText = $"select * from fn_active_fund('{defaultDate}') where [FUND ID PHARUS] = {Id}";
                }

                else
                {
                    command.CommandText = $"select * from fn_active_fund('{chosenDate?.ToString("yyyyMMdd")}') where [FUND ID PHARUS] = {Id}";
                }

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }
        public List<string[]> GetActiveFundWithDateById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from fn_active_fund_modifyview('{defaultDate}') where [FUND ID PHARUS] = {Id}";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public List<string[]> GetFundSubFunds(int Id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"select * from ActivesubfundforSpecificFundAtDate('{defaultDate}', {Id})";

                return CreateModel.CreateModelWithHeadersAndValue(command);
            }
        }

        public void ExecuteEditFund(List<string[]> fundsProperties, int fStatusId,
                                                int fLegalFormId, int fLegalTypeId,
                                                int fLegalVehicleId, int fCompanyTypeId)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                command.CommandText = $"EXEC sp_modify_fund{string.Join("", fundsProperties)}";
            }
        }
    }
}