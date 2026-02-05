namespace CookBook.Contracts;

public record RecipeInUserVm(int Id, string Title, double? AverageRating);
public record RatingInUserVm(int RecipeId, string RecipeTitle, int Value);
public record UserVm(int Id, string Login, List<RecipeInUserVm> Recipes, List<RatingInUserVm> Ratings);
public record UserInListVm(int Id, string Login);
public record ListOfUsers(List<UserInListVm> Users);
public record UpdateUserDto(string Login);
public record SignUpDto(string Login, string Password);
public record LogInDto(string Login, string Password);
