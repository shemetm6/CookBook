namespace CookBook.Contracts;

// (для себя) Не забыть добавить рейтинг, когда он появится
public record RecipeInUserVm(int Id, string Title);
public record UserVm(int Id, string Login, List<RecipeInUserVm> Recipes);
public record UserInListVm(int Id, string Login);
public record ListOfUsers(List<UserInListVm> Users);
public record CreateUserDto(string Login, string Password);
// Почему изменяем только логин? Может дать юзеру возможность поменять себе пароль?
public record UpdateUserDto(string Login);
