﻿// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
// Utility class for extracting table data
// as PDF and Excel

// Created: 10/2019
// Author:  Philip Shishov
// NugetPackages : itext7 7.1.8, epplus.core 1.5.4

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
namespace DataGate.Web.Utilities
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;

    using OfficeOpenXml;

    using iText.Layout;
    using iText.IO.Image;
    using iText.Kernel.Pdf;
    using iText.Kernel.Geom;
    using iText.Layout.Element;
    using iText.Layout.Properties;

    // _____________________________________________________________
    public class ExtractTable
    {
        // ---------------------------------------------------------
        //
        // Names for different files when created
        private const string ActiveFunds = "ActiveFunds";
        private const string ActiveSubFunds = "ActiveSubFunds";
        private const string ActiveShareClasses = "ActiveShareClasses";

        // ________________________________________________________
        //
        // Extract table data as Excel
        // and preparing for download
        // in controller as filestreamresult
        public static FileStreamResult ExtractTableAsExcel(
                                                           List<string[]> entities,
                                                           string typeName,
                                                           string controllerName)
        {
            FileStreamResult fileStreamResult;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = null;
                string correctTypeName = GetCorrectTypeName(typeName, controllerName);

                worksheet = package.Workbook.Worksheets.Add($"{correctTypeName}");

                var tableHeaders = entities.Take(1);

                int counter = 0;

                foreach (var tableHeader in tableHeaders)
                {
                    foreach (var headerValue in tableHeader)
                    {
                        counter++;
                        worksheet.Cells[1, counter].Value = headerValue;
                    }
                }

                for (int row = 1; row < entities.Count; row++)
                {
                    for (int col = 0; col < entities[row].Length; col++)
                    {
                        worksheet.Cells[row + 1, col + 1].Value = Convert.ToString(entities[row][col]);
                    }
                }

                package.Save();

                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                fileStreamResult = new FileStreamResult(stream, "application/excel")
                {
                    FileDownloadName = $"{correctTypeName}.xlsx",
                };

                return fileStreamResult;
            }
        }

        // ________________________________________________________
        //
        // Extract table data as PDF
        // and preparing for download
        // in controller as filestreamresult
        public static FileStreamResult ExtractTableAsPdf(
                                                         List<string[]> entities,
                                                         DateTime? chosenDate,
                                                         IWebHostEnvironment hostingEnvironment,
                                                         string typeName,
                                                         string controllerName)
        {
            string correctTypeName = GetCorrectTypeName(typeName, controllerName);

            int tableLength = entities[0].Length;

            FileStreamResult fileStreamResult;
            Stream stream = new MemoryStream();
            PdfWriter writer = new PdfWriter(stream);
            writer.SetCloseStream(false);

            PdfDocument pdfDoc = new PdfDocument(writer);

            // Funds table format settings
            pdfDoc.SetDefaultPageSize(PageSize.A3.Rotate());

            // SubFunds table format settings
            // ShareClasses table format settings

            Document document = new Document(pdfDoc);

            string sfile = hostingEnvironment.WebRootPath + "/images/Logo_Pharus_small.jpg";
            ImageData data = ImageDataFactory.Create(sfile);

            Image img = new Image(data);

            Table table = new Table(tableLength);

            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetFontSize(10);

            for (int row = 0; row < 1; row++)
            {
                for (int col = 0; col < entities[0].Length; col++)
                {
                    string s = entities[row][col];
                    if (s == null)
                    {
                        s = " ";
                    }

                    Cell cell = new Cell();
                    cell.Add(new Paragraph(s));
                    cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    cell.SetBold();

                    table.AddHeaderCell(cell);
                }
            }

            for (int row = 1; row < entities.Count; row++)
            {
                for (int col = 0; col < entities[0].Length; col++)
                {
                    string s = entities[row][col];
                    if (s == null)
                    {
                        s = " ";
                    }

                    table.AddCell(new Paragraph(s));
                }
            }

            document.Add(img);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"List of {correctTypeName} as of " + chosenDate?.ToString("dd MMMM yyyy")));
            document.Add(new Paragraph(" "));
            document.Add(table);
            document.Close();

            stream.Position = 0;
            fileStreamResult = new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{correctTypeName}.pdf",
            };
            return fileStreamResult;
        }

        // ________________________________________________________
        //
        // Method for choosing the correct name
        // for file to be downloaded
        // based on controller and
        // view model name
        private static string GetCorrectTypeName(string typeName, string controllerName)
        {
            string correctTypeName = string.Empty;

            if (controllerName == "Funds")
            {
                correctTypeName = typeName == "EntitiesViewModel" ? ActiveFunds : ActiveSubFunds;
            }
            else if (controllerName == "SubFunds")
            {
                correctTypeName = typeName == "EntitiesViewModel" ? ActiveSubFunds : ActiveShareClasses;
            }
            else if (controllerName == "ShareClasses")
            {
                correctTypeName = ActiveShareClasses;
            }

            return correctTypeName;
        }
    }
}