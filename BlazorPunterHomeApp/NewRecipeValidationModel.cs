using Newtonsoft.Json;
using PunterHomeDomain.Enums;
using RazorShared.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp
{
    public class NewRecipeValidationModel
    {
        [Required]
        [StringLength(64, ErrorMessage = "Name to long")]
        public string Name { get; set; }

        [Required]
        [RecipeTypeValidation]
        public ERecipeType Type { get; set; }

        [JsonIgnore]
        public List<ERecipeType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(ERecipeType)).Cast<ERecipeType>().ToList();
    }

    public class NewProductValidationModel
    {
        [Required]
        [StringLength(64, ErrorMessage = "Name to long")]
        public string Name { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Volume invalid (1-10000)")]
        public int UnitQuantity { get; set; } = 0;

        [Required]
        [UnitMeasurementTypeValidation]
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public string Barcode { get; set; }

        [JsonIgnore]
        public List<EUnitMeasurementType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitMeasurementType)).Cast<EUnitMeasurementType>().ToList();
    }
}
