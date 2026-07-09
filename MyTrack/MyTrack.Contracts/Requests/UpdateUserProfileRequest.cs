namespace MyTrack.Contracts.Requests;

/// <summary>
/// Represents a request to update user profile details.
/// </summary>
public class UpdateUserProfileRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string HomeAddress { get; set; } = string.Empty;
    public string WorkAddress { get; set; } = string.Empty;
}