using PunterHomeDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Enums;
using EUnitMeasurementType = Enums.EUnitMeasurementType;

namespace RazorShared.Validations
{
    public class RecipeTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {


            if (!(value is ERecipeType recipeType))
            {
                return new ValidationResult("Ivalid type", new[] { validationContext.MemberName });
            }

            if (recipeType == ERecipeType.None)
            {

                return new ValidationResult("Selecteer recept soort", new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }

    public class UnitMeasurementTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {


            if (!(value is EUnitMeasurementType productType))
            {
                return new ValidationResult("Ivalid type", new[] { validationContext.MemberName });
            }

            if (productType == EUnitMeasurementType.None)
            {

                return new ValidationResult("Selecteer recept soort", new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
