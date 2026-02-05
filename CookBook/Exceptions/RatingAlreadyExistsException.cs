namespace CookBook.Exceptions
{
    public class RatingAlreadyExistsException : Exception
    {
        public RatingAlreadyExistsException(int recipeId) : base($"You have already rated recipe with id = {recipeId}!") { }
    }
}
