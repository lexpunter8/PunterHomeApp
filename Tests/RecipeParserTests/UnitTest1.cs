using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeParsers;
using System.Threading.Tasks;

namespace RecipeParserTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            await new ProjectGezondParser("https://www.projectgezond.nl/turkse-pizza/", NSubsstitute).Parse();
        }
    }
}
