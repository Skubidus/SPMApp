namespace SPMLibrary.Models;

//#nullable disable
public class EntryModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string? WebsiteUrl { get; set; }
    public string? Notes { get; set; }
    public List<TagModel> Tags { get; set; } = new();
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
}
//#nullable restore