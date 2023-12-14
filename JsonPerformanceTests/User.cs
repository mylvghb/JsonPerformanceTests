namespace JsonPerformanceTests;

public class User(Guid id, string firstName, string lastName, string fullName, string username, string email)
{
    public Guid Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string FullName { get; set; } = fullName;
    public string Username { get; set; } = username;
    public string Email { get; set; } = email;
}