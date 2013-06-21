using System;
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

            commandBuilder.Where("A.Column = @Start", expectedValue);

            Assert.That(commandBuilder.Build().Parameters["Start"], Is.EqualTo(expectedValue));
        }

        [Test]
        public void SetsMultipleParameters()
        {
            var expectedValue = new DateTime(2013, 1, 1);

            var commandBuilder = new SqlCommandBuilder("SELECT * FROM TableA A");

            commandBuilder.Where("A.Column BETWEEN @Start AND @End", new DateTime(2012, 1, 1), expectedValue);

            Assert.That(commandBuilder.Build().Parameters["Start"], Is.EqualTo(expectedValue));
        }
    }
}
