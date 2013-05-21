using LeanKit.APIClient.API;
using NUnit.Framework;

namespace LeanKit.APIClient.Tests
{
    [TestFixture]
    class IntegrationTests
    {
        private const string USERNAME = "";
        private const string PASSWORD = "";
        private const string ACCOUNT = "";
        private const string BOARD_ID = "";

        [Test]
        public void SetsBoardId ()
        {
            var apiCaller = new ApiCaller
                {
                    Account = ACCOUNT,
                    BoardId = BOARD_ID,
                    Credentials = new ApiCredentials
                    {
                        Username = USERNAME,
                        Password = PASSWORD
                    }
                };

            var leankitBoard = apiCaller.GetBoardResponse<LeankitBoard>("Boards");

            Assert.That(leankitBoard.Id, Is.EqualTo(32482312));
        }

        [Test]
        public void SetsCardHistory()
        {
            var apiCaller = new ApiCaller
            {
                Account = ACCOUNT,
                BoardId = BOARD_ID,
                Credentials = new ApiCredentials
                {
                    Username = USERNAME,
                    Password = PASSWORD
                }
            };

            var leankitBoard = apiCaller.GetHistoryResponse<LeankitBoard>(40816485);

            Assert.That(leankitBoard, Is.EqualTo(32482312));
        }
    }
}
