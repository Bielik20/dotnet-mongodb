using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_mongodb.Models
{
    public class ListSample
    {
        public ObjectId Id { get; set; }
        public ICollection<AntiCoagulantAntiPlateletAndFibrinolyticEnum> ListOfEnums { get; set; }
    }

    public enum AntiCoagulantAntiPlateletAndFibrinolyticEnum
    {
        [Display(Name = "Acenocoumarol")]
        A0,
        [Display(Name = "Clopidogrel")]
        A1,
        [Display(Name = "Dabigatran")]
        A2,
        [Display(Name = "Dalteparin")]
        A3,
        [Display(Name = "Eltrombopag")]
        A4,
        [Display(Name = "Enoxaparin")]
        A5,
        [Display(Name = "Heparin")]
        A6,
        [Display(Name = "Phenprocoumon")]
        A7,
        [Display(Name = "Prasugrel")]
        A8,
        [Display(Name = "Rivaroxaban")]
        A9,
        [Display(Name = "Streptokinase")]
        A10,
        [Display(Name = "Tricagrelor")]
        A11,
        [Display(Name = "Warfarin")]
        A12,
    }
}
