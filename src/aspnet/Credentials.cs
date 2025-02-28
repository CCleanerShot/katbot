using System.ComponentModel.DataAnnotations;

public class Credentials
{
    /// <summary>
    /// The session key for the user.
    /// </summary>
    public string SessionKey { get; set; } = "123123";

    /// <summary>
    /// The Discord ID of the user.
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    [Required]
    public string Username { get; set; } = "Test";

    /// <summary>
    /// The password of the user. This will likely be given to by me.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "123";

    public bool IsValid
    {
        get => true;
    }
}