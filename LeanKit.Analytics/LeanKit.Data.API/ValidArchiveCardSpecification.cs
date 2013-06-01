using System;
using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public class ValidArchiveCardSpecification : IValidateLeankitCards
    {
        public bool IsSatisfiedBy(LeankitBoardCard card)
        {
            return !String.IsNullOrWhiteSpace(card.Title) && !card.Title.Contains("Cards older than");
        }
    }
}