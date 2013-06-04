using LeanKit.ReleaseManager.Models;
using NUnit.Framework;

namespace LeanKit.Analytics.Tests.Controllers
{
    [TestFixture]
    public class ColourPaletteTests
    {
        [Test]
        public void NextReturnsFirstColour()
        {
            var colours = new[] { "#F00" };

            Assert.That(new ColourPalette(colours).Next(), Is.EqualTo("#F00"));
        }

        [Test]
        public void NextReturnsSecondColour()
        {
            var colours = new[] { "#F00", "#0F0" };

            var colourPalette = new ColourPalette(colours);
            colourPalette.Next();

            Assert.That(colourPalette.Next(), Is.EqualTo("#0F0"));
        }

        [Test]
        public void NextLoopsThroughColours()
        {
            var colours = new[] { "#F00", "#0F0" };

            var colourPalette = new ColourPalette(colours);
            colourPalette.Next();
            colourPalette.Next();

            Assert.That(colourPalette.Next(), Is.EqualTo("#F00"));
        }
    }
}