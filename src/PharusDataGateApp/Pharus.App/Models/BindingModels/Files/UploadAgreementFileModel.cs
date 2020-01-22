﻿// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
// Binding model for agreement file upload

// Created: 01/2020
// Author:  Philip Shishov

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
namespace Pharus.App.Models.BindingModels.Files
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UploadAgreementFileModel : BaseUploadFileBindingModel
    {
        [Required]
        public string AgrType { get; set; }

        [Required(ErrorMessage = "Contract Date cannot be empty")]
        [Display(Name = "Contract Date")]
        public DateTime ContractDate { get; set; }

        [Required(ErrorMessage = "Activation Date cannot be empty")]
        [Display(Name = "Activation Date")]
        public DateTime ActivationDate { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Status { get; set; }
    }
}