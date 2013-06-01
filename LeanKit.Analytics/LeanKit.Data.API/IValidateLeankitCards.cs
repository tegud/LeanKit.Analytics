using LeanKit.APIClient.API;

namespace LeanKit.Data.API
{
    public interface IValidateLeankitCards
    {
        bool IsSatisfiedBy(LeankitBoardCard card);
    }
}