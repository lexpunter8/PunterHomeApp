using AngleSharp;
using AngleSharp.Html.Dom;
using PunterHomeApp.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Enums;

namespace RecipeParsers
{
    public class ProjectGezondParser : IRecipeParser
    {
        private Dictionary<EUnitMeasurementType, string[]> typeMapping = new Dictionary<EUnitMeasurementType, string[]>
        {
            { EUnitMeasurementType.Piece, new string[] { "teentje", "wraps"}},
            { EUnitMeasurementType.theelepel, new string[] { "tl" }},
            { EUnitMeasurementType.Gr, new string[] { "gr"}},
        };


        private readonly string url;
        private readonly IProductService productService;
        private readonly IRecipeService recipeService;

        private EUnitMeasurementType GetMeasurementType(string s)
        {

            var existingMapping = typeMapping.FirstOrDefault(f => f.Value.Any(a => a.ToLower() == s.ToLower()));

            if (existingMapping.Key == EUnitMeasurementType.None)
            {
                return EUnitMeasurementType.Piece;
            }
                return existingMapping.Key;

            return EUnitMeasurementType.Piece;
        }

        public async Task<RecipeAggregate> Parse(string url)
        {
            using (WebClient client = new WebClient())
            {
                var config = Configuration.Default.WithDefaultLoader();
                var document = await BrowsingContext.New(config).OpenAsync(url);

                var result = document.All.Where(m =>
                                                    m.LocalName == "div" &&
                                                    m.HasAttribute("class") &&
                                                    m.GetAttribute("class").Contains("entry-content")
                                                );

                var header = document.All.Where(m =>
                                                   m.LocalName == "header" &&
                                                   m.HasAttribute("class") &&
                                                   m.GetAttribute("class").Contains("entry-header")
                                               );

                var ingredientList = result.First().Children.SkipWhile(m => !m.InnerHtml.Contains("Dit heb je nodig")).Skip(1).Take(1);


                RecipeAggregate recipeAggregate = new(header.First().Children.Skip(1).First().InnerHtml);
                foreach (var item in ingredientList.First().Children)
                {
                    var splitted = item.InnerHtml.Split(" ");
                    
                    var quantity = splitted[0];
                    var measurementType = GetMeasurementType(splitted[1]);
                    var pName = string.Join(" ", splitted.Skip(measurementType == EUnitMeasurementType.Piece ? 1 : 2));

                    if (item.Children.Any(a => a.GetType().ToString() ==  "AngleSharp.Html.Dom.HtmlAnchorElement"))
                    {
                        pName = item.Children.First(a => a.GetType().ToString() == "AngleSharp.Html.Dom.HtmlAnchorElement").InnerHtml;
                    }
                    if (pName.ToLower().StartsWith("<a href"))
                    {

                    }
                    if (!double.TryParse(quantity, out double x))
                    {
                        continue;
                    }

                    recipeAggregate.AddIngredient(new IngredientValueObject
                    {
                        ProductName = pName,
                        UnitQuantity = x,
                        UnitQuantityType = measurementType 
                    });
                }


                var steps = result.First().Children.SkipWhile(m => !m.InnerHtml.Contains("Zo maak je het")).Skip(2).Take(1);


                foreach (var item in steps.First().Children)
                {
                    recipeAggregate.AddStep(item.InnerHtml);
                }

                return recipeAggregate;
            }
        }
    }
}
