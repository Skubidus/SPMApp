namespace SPMLibrary.Models;

public class EntryModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string? WebsiteUrl { get; set; }
    public string? Notes { get; set; }
    public List<TagModel> Tags { get; set; } = [];
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not EntryModel
            || obj is null)
        {
            return false;
        }

        var model = (EntryModel)obj;

        if (model.Id != this.Id
            || model.Title != this.Title
            || model.Username != this.Username
            || model.Password != this.Password
            || model.WebsiteUrl != this.WebsiteUrl
            || model.Notes != this.Notes
            || AreTagListsEqual(model.Tags, this.Tags) == false
            || model.DateCreated != this.DateCreated
            || model.DateModified != this.DateModified)
        {
            return false;
        }

        return true;
    }

    public bool EqualsWithoutId(object? obj)
    {
        if (obj is not EntryModel
            || obj is null)
        {
            return false;
        }

        var model = (EntryModel)obj;

        if (model.Title != this.Title
            || model.Username != this.Username
            || model.Password != this.Password
            || model.WebsiteUrl != this.WebsiteUrl
            || model.Notes != this.Notes
            || AreTagListsEqual(model.Tags, this.Tags) == false
            || model.DateCreated != this.DateCreated
            || model.DateModified != this.DateModified)
        {
            return false;
        }

        return true;
    }

    public static bool AreTagListsEqual(IEnumerable<TagModel> list1, IEnumerable<TagModel> list2)
    {
        var output = list1.SequenceEqual(list2);
        return output;
    }

    public EntryModel Clone()
    {
        EntryModel output = new()
        {
            Id = this.Id,
            Title = this.Title,
            Username = this.Username,
            Password = this.Password,
            WebsiteUrl = this.WebsiteUrl,
            Notes = this.Notes,
            Tags = this.Tags.ToList(),
            DateCreated = this.DateCreated,
            DateModified = this.DateModified
        };

        return output;
    }
}