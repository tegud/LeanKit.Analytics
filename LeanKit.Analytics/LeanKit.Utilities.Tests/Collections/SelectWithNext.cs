using System;
using System.Collections.Generic;
using System.Linq;
using LeanKit.Utilities.Collections;
using NUnit.Framework;

namespace LeanKit.Utilities.Tests.Collections
{
    [TestFixture]
    public class SelectWithNextTests
    {
        [Test]
        public void SelectsCurrent()
        {
            var actualCurrent = 0;
            var listOfItems = new List<int>
                {
                    1
                };

            var selectedItems = listOfItems.SelectWithNext((current, next) =>
            {
                actualCurrent = current;
                return 0;
            }).ToArray();

            Assert.That(actualCurrent, Is.EqualTo(1));
        }

        [Test]
        public void SelectsSecondCurrent()
        {
            var actualCurrent = 0;
            var listOfItems = new List<int>
                {
                    1, 2
                };

            var selectedItems = listOfItems.SelectWithNext((current, next) =>
            {
                actualCurrent = current;
                return 0;
            }).ToArray();

            Assert.That(actualCurrent, Is.EqualTo(2));
        }

        [Test]
        public void SelectsNextItem()
        {
            var actualNext = 0;
            var listOfItems = new List<int>
                {
                    1, 2
                };

            var selectedItems = listOfItems.SelectWithNext((current, next) =>
            {
                if (actualNext == 0)
                {
                    actualNext = next;
                }
                return 0;
            }).ToArray();

            Assert.That(actualNext, Is.EqualTo(2));
        }

        [Test]
        public void SelectsLastItemNextAsDefault()
        {
            var actualNext = -1;
            var listOfItems = new List<int>
                {
                    1, 2
                };

            var selectedItems = listOfItems.SelectWithNext((current, next) =>
            {
                actualNext = next;
                return 0;
            }).ToArray();

            Assert.That(actualNext, Is.EqualTo(0));
        }

        [Test]
        public void SelectsLastItemNextAsNullWhenEnumeratingObjecrs()
        {
            AClass actualNext = null;
            var listOfItems = new List<AClass>
                {
                    new AClass(), new AClass()
                };

            var selectedItems = listOfItems.SelectWithNext((current, next) =>
            {
                actualNext = next;
                return 0;
            }).ToArray();

            Assert.That(actualNext, Is.Null);
        }

        public class AClass
        {
            public DateTime DT { get; set; }
        }
    }
}
