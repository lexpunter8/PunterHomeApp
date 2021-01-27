using Newtonsoft.Json;
using PunterHomeDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp
{
    public class NewRecipeValidationModel
    {
        [Required]
        [StringLength(64, ErrorMessage = "Name to long")]
        public string Name { get; set; }

        [Required]
        public ERecipeType Type { get; set; }

        [JsonIgnore]
        public List<ERecipeType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(ERecipeType)).Cast<ERecipeType>().ToList();
    }
}
