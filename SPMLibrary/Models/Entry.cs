namespace SPMLibrary.Models;

#nullable disable
public class Entry
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string WebsiteUrl { get; set; }
    public string Notes { get; set; }
    public List<Tag> Tags { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
}
#nullable restore