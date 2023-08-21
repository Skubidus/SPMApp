namespace SPMLibrary.Models;

public class TagModel
{
    public int Id { get; set; }
#nullable disable
    public string Title { get; set; }
#nullable restore

    public override bool Equals(object? obj)
    {
        if (obj is not TagModel
            || obj is null)
        {
            return false;
        }
        
        var tag = obj as TagModel;

        if (tag!.Id != this.Id
            || tag.Title != this.Title)
        {
            return false;
        }

        return true;
    }
}