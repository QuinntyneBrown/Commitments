namespace IdentityService.Core.Messages;

internal class UserMetadataMessage
{
    public string Username { get; set; }
    public string MetadataPropertyName { get; set; }
    public string MetadataPropertyValue { get; set; }
}
