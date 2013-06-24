using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LeanKit.Data.SQL.Tests
{
    [TestFixture]
    public class SqlCommandBuilderTests
    {
        [Test]
        public void SetsSelectSql()
        {
            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            Assert.That(commandBuilder.Build().Sql, Is.StringStarting("SELECT * FROM TableA A"));
        }

        [Test]
        public void SetsOrderBySql()
        {
            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder.OrderBy("A.Column", SqlCommandOrderDirection.Descending);

            Assert.That(commandBuilder.Build().Sql, Is.StringEnding(" ORDER BY A.Column DESC"));
        }

        [Test]
        public void SetsSecondOrderBy()
        {
            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder.OrderBy("A.Column", SqlCommandOrderDirection.Descending);
            commandBuilder.OrderBy("A.Column2", SqlCommandOrderDirection.Ascending);

            Assert.That(commandBuilder.Build().Sql, Is.StringEnding(" ORDER BY A.Column DESC, A.Column2 ASC"));
        }

        [Test]
        public void SetsWhereClause()
        {
            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder.Where("A.Column = @Start", null);

            Assert.That(commandBuilder.Build().Sql, Is.StringEnding(" WHERE A.Column = @Start"));
        }

        [Test]
        public void SetsParameterForWhereClause()
        {
            var expectedValue = new DateTime(2013, 1, 1);

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder.Where("A.Column = @Start", new Dictionary<string, object>
                {
                    {"Start", expectedValue}
                });

            Assert.That(commandBuilder.Build().Parameters["Start"], Is.EqualTo(expectedValue));
        }

        [Test]
        public void SetsMultipleParameters()
        {
            var expectedValue = new DateTime(2013, 1, 1);

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder.Where("A.Column BETWEEN @Start AND @End", new Dictionary<string, object>
                {
                    { "Start", new DateTime(2012, 1, 1) },
                    { "End", expectedValue }
                });

            Assert.That(commandBuilder.Build().Parameters["End"], Is.EqualTo(expectedValue));
        }

        [Test]
        public void AndSetsWhereToIncludeAndClause()
        {
            const string expectedWhereClause = "WHERE A.Column BETWEEN @Start AND @End AND A.Column2 = @SiteID";

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder
                .Where("A.Column BETWEEN @Start AND @End", new Dictionary<string, object>
                    {
                        {"Start", new DateTime(2012, 1, 1)},
                        {"End", new DateTime(2013, 1, 1)}
                    })
                .And("A.Column2 = @SiteID", new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    });

            Assert.That(commandBuilder.Build().Sql, Is.StringEnding(expectedWhereClause));
        }

        [Test]
        public void OrSetsWhereToIncludeAndClause()
        {
            const string expectedWhereClause = "WHERE A.Column BETWEEN @Start AND @End OR A.Column2 = @SiteID";

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder
                .Where("A.Column BETWEEN @Start AND @End", new Dictionary<string, object>
                    {
                        {"Start", new DateTime(2012, 1, 1)},
                        {"End", new DateTime(2013, 1, 1)}
                    })
                .Or("A.Column2 = @SiteID", new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    });

            Assert.That(commandBuilder.Build().Sql, Is.StringEnding(expectedWhereClause));
        }

        [Test]
        public void AndAppendsParameters()
        {
            const int expectedValue = 1;

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder
                .Where("A.Column BETWEEN @Start AND @End", new Dictionary<string, object>
                    {
                        {"Start", new DateTime(2012, 1, 1)},
                        {"End", new DateTime(2013, 1, 1)}
                    })
                .And("A.Column2 = @SiteID", new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    });

            Assert.That(commandBuilder.Build().Parameters["SiteID"], Is.EqualTo(expectedValue));
        }

        [Test]
        public void OrAppendsParameters()
        {
            const int expectedValue = 1;

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder
                .Where("A.Column BETWEEN @Start AND @End", new Dictionary<string, object>
                    {
                        {"Start", new DateTime(2012, 1, 1)},
                        {"End", new DateTime(2013, 1, 1)}
                    })
                .Or("A.Column2 = @SiteID", new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    });

            Assert.That(commandBuilder.Build().Parameters["SiteID"], Is.EqualTo(expectedValue));
        }

        [Test]
        public void AndThenOrCanSpecifyTheSameParameters()
        {
            const int expectedValue = 1;

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder
                .Where("A.Column BETWEEN @Start AND @End", new Dictionary<string, object>
                    {
                        {"Start", new DateTime(2012, 1, 1)},
                        {"End", new DateTime(2013, 1, 1)}
                    })
                .Or("A.Column2 = @SiteID", new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    })
                .And("A.Column2 = @SiteID", new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    });

            Assert.That(commandBuilder.Build().Parameters["SiteID"], Is.EqualTo(expectedValue));
        }

        [Test]
        public void ParametersSetsParameterValues()
        {
            const int expectedValue = 1;

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder
                .Where("A.Column = @SiteID")
                .Parameters(new Dictionary<string, object>
                    {
                        {"SiteID", 1}
                    });

            Assert.That(commandBuilder.Build().Parameters["SiteID"], Is.EqualTo(expectedValue));
        }
    }
}
