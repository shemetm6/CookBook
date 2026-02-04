namespace CookBook.Contracts;

// (todo) Внедрить рейтинг
public record RecipeInUserVm(int Id, string Title);
public record UserVm(int Id, string Login, List<RecipeInUserVm> Recipes);
public record UserInListVm(int Id, string Login);
public record ListOfUsers(List<UserInListVm> Users);
public record UpdateUserDto(string Login);
public record SignUpDto(string Login, string Password);
public record LogInDto(string Login, string Password);
