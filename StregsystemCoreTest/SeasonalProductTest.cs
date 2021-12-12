using StregsystemCore;
using System;
using System.Globalization;
using Xunit;

namespace StregsystemCoreTest
{
    public class SeasonalProductTest
    {
        [Theory]
        [InlineData("07/12/2021", false)]
        [InlineData("08/12/2021", true)]
        [InlineData("10/12/2021", true)]
        [InlineData("11/12/2021", false)]
        public void IsActiveWithinDate(string dateTimeString, bool shouldBeActive)
        {
            StaticDateProvider staticDateProvider = new StaticDateProvider(
                DateTime.Parse(dateTimeString, CultureInfo.GetCultureInfo("da-DK"))
            );

            SeasonalProduct seasonalProduct = new SeasonalProduct(
                1,
                "test",
                100,
                true,
                false,
                new DateTime(2021, 12, 8),
                new DateTime(2021, 12, 10),
                staticDateProvider
            );
            Assert.Equal(shouldBeActive, seasonalProduct.Active);
        }
    }
}
