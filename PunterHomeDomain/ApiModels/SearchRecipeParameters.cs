using PunterHomeDomain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.ApiModels
{
    public class SearchRecipeParameters
    {
        public string Name { get; set; }
        public ERecipeType Type { get; set; }
    }
}
