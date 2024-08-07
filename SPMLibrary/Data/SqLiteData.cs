using SPMLibrary.DataAccess;
using SPMLibrary.Models;

using System.Reflection.Metadata.Ecma335;

namespace SPMLibrary.Data;

public class SqLiteData : ISqLiteData
{
    private readonly ISqLiteDataAccess _db;
    private const string _connectionStringName = "SqLiteDb";

    public SqLiteData(ISqLiteDataAccess db)
    {
        _db = db;
    }

    public List<EntryModel> GetAllEntries()
    {
        string sql = @" SELECT Id, Title, Username, Password, WebsiteUrl, Notes, DateCreated, DateModified
                        FROM Entries e;";

        var entries = _db.SqlQuery<EntryModel, dynamic>(sql, new { }, _connectionStringName);

        foreach (var entry in entries)
        {
            sql = @"SELECT t.Id, t.Title
                    FROM Tags t
                    JOIN EntriesTags et
                        ON et.TagId = t.Id
                        AND et.EntryId = @EntryId;";

            entry.Tags = _db.SqlQuery<TagModel, dynamic>(
                sql,
                new { EntryId = entry.Id },
                _connectionStringName);
        }

        return entries;
    }

    public EntryModel? GetEntryById(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);

        string sql = @" SELECT Id, Title, Username, Password, WebsiteUrl, Notes, DateCreated, DateModified
                        FROM Entries e
                        WHERE e.Id = @Id;";

        var entry = _db.SqlQuery<EntryModel, dynamic>(
            sql,
            new { Id = id },
            _connectionStringName).FirstOrDefault();

        if (entry is not null)
        {
            entry.Tags = GetTagsForEntry(entry.Id);
        }

        return entry;
    }

    private List<TagModel> GetTagsForEntry(int entryId)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(entryId);

        string sql = @"SELECT t.Id, t.Title
                    FROM Tags t
                    JOIN EntriesTags et
                        ON et.TagId = t.Id
                        AND et.EntryId = @EntryId;";

        var tags = _db.SqlQuery<TagModel, dynamic>(
            sql,
            new { EntryId = entryId },
            _connectionStringName);

        return tags;
    }

    public void InsertEntry(EntryModel entry)
    {
        entry.DateCreated = DateTime.Now;
        entry.DateModified = DateTime.Now;

        string sql = @"INSERT INTO Entries (Title, Username, Password, WebsiteUrl, Notes, DateCreated, DateModified)
                       VALUES(@Title, @Username, @Password, @WebsiteUrl, @Notes, @DateCreated, @DateModified);";

        _db.SqlExecute<dynamic>(
            sql,
            new
            {
                entry.Title,
                entry.Username,
                entry.Password,
                entry.WebsiteUrl,
                entry.Notes,
                entry.DateCreated,
                entry.DateModified
            },
            _connectionStringName);

        sql = @"SELECT Id FROM Entries
                WHERE Title = @Title AND Username = @Username AND Password = @Password AND WebsiteUrl = @WebsiteUrl
                AND Notes = @Notes AND DateCreated = @DateCreated AND DateModified = @DateModified;";

        entry.Id = _db.SqlQuery<int, dynamic>(
            sql,
            new
            {
                entry.Title,
                entry.Username,
                entry.Password,
                entry.WebsiteUrl,
                entry.Notes,
                entry.DateCreated,
                entry.DateModified
            },
            _connectionStringName)
            .FirstOrDefault();

        if (entry.Id == 0)
        {
            throw new InvalidOperationException("Recently added entry was expected but not found in the database.");
        }

        var tagsNotAlreadyInDb = GetTagsNotAlreadyInDb(entry.Tags);
        tagsNotAlreadyInDb.ForEach(tag => InsertTag(tag));
        tagsNotAlreadyInDb.ForEach(tag => tag.Id = GetTagId(tag));

        entry.Tags.ForEach(tag => AddTagReferenceToEntry(tag, entry));
    }

    public void UpdateEntry(EntryModel entry)
    {
        if (entry.Id <= 0)
        {
            throw new InvalidOperationException("Invalid entry ID.");
        }

        entry.DateModified = DateTime.Now;

        string sql = @"UPDATE Entries
                       SET Title = @Title, Username = @Username, Password = @Password,
                       WebsiteUrl = @WebsiteUrl, Notes = @Notes, DateModified = @DateModified
                       WHERE Id = @Id;";

        _db.SqlExecute<dynamic>(sql,
            new
            {
                entry.Title,
                entry.Username,
                entry.Password,
                entry.WebsiteUrl,
                entry.Notes,
                entry.DateModified,
                entry.Id
            },
            _connectionStringName);

        var tags = GetTagsForEntry(entry.Id);

        var tagsToAdd = GetTagsToAddToEntry(tags, entry.Tags);
        var tagsToRemove = GetTagsToRemoveFromEntry(tags, entry.Tags);

        var tagsNotAlreadyInDb = GetTagsNotAlreadyInDb(tagsToAdd);

        tagsNotAlreadyInDb.ForEach(tag => InsertTag(tag));

        tagsNotAlreadyInDb.ForEach(tag => tag.Id = GetTagId(tag));

        tagsToAdd.ForEach(tag => AddTagReferenceToEntry(tag, entry));

        tagsToRemove.ForEach(tag => RemoveTagReferenceFromEntry(tag, entry));

        DeleteUnusedTags(tagsToRemove);
    }

    private void DeleteTag(TagModel tag)
    {
        if (tag.Id <= 0)
        {
            throw new InvalidOperationException("Invalid tag ID. Must be greater than 0.");
        }

        RemoveAllTagReferencesForTag(tag);

        string sql = @"DELETE FROM Tags
                       WHERE Id = @TagId;";

        _db.SqlExecute<dynamic>(
            sql,
            new { TagId = tag.Id },
            _connectionStringName);
    }

    private void RemoveAllTagReferencesForTag(TagModel tag)
    {
        if (tag.Id <= 0)
        {
            throw new InvalidOperationException("Invalid tag ID. Must be greater than 0.");
        }

        string sql = @"DELETE FROM EntriesTags
                       WHERE TagId = @TagId;";

        _db.SqlExecute<dynamic>(
            sql,
            new { TagId = tag.Id },
            _connectionStringName);
    }

    private void RemoveTagReferenceFromEntry(TagModel tag, EntryModel entry)
    {
        if (tag.Id <= 0)
        {
            throw new InvalidOperationException("Tag ID can not be smaller or equal to 0.");
        }

        if (entry.Id <= 0)
        {
            throw new InvalidOperationException("Entry ID can not be smaller or equal to 0.");
        }

        string sql = @"DELETE FROM EntriesTags
                       WHERE TagId = @TagId
                       AND EntryId = @EntryId;";

        _db.SqlExecute<dynamic>(
            sql,
            new
            {
                TagId = tag.Id,
                EntryId = entry.Id
            },
            _connectionStringName);

        DeleteUnusedTags(entry.Tags);

        var tagToRemove = entry.Tags.FirstOrDefault(t => t.Id == tag.Id);

        if (tagToRemove is not null)
        {
            entry.Tags.Remove(tagToRemove);
        }
    }

    private int GetTagId(TagModel tag)
    {
        int output = 0;

        string sql = "";
        sql = @"SELECT Id, Title FROM Tags
                WHERE Title = @Title;";

        var tempTag = _db.SqlQuery<TagModel, dynamic>(
            sql,
            new { tag.Title },
            _connectionStringName)
            .FirstOrDefault();

        if (tempTag is null)
        {
            return 0;
        }

        output = tempTag.Id;

        return output;
    }

    private void AddTagReferenceToEntry(TagModel tag, EntryModel entry)
    {
        string sql = "";

        sql = @"INSERT INTO EntriesTags (TagId, EntryId)
                VALUES (@TagId, @EntryId);";

        _db.SqlExecute<dynamic>(
            sql,
            new { TagId = tag.Id, EntryId = entry.Id },
            _connectionStringName);
    }

    private List<TagModel> GetTagsToAddToEntry(IEnumerable<TagModel> tagsSource, IEnumerable<TagModel> tagsTarget)
    {
        List<TagModel> output = [];

        foreach (var tag in tagsTarget)
        {
            if (tagsSource.Contains(tag))
            {
                continue;
            }

            output.Add(tag);
        }

        return output;
    }

    private List<TagModel> GetTagsToRemoveFromEntry(IEnumerable<TagModel> tagsSource, IEnumerable<TagModel> tagsTarget)
    {
        List<TagModel> output = [];

        foreach (var tag in tagsSource)
        {
            if (tagsTarget.Contains(tag))
            {
                continue;
            }

            output.Add(tag);
        }

        return output;
    }

    private List<TagModel> GetTagsNotAlreadyInDb(IEnumerable<TagModel> tagsSource)
    {
        var output = new List<TagModel>();

        foreach (var tag in tagsSource)
        {
            string sql = "";

            sql = @"SELECT Id, Title
                    FROM Tags
                    WHERE Id = @Id;";

            var resultTag = _db.SqlQuery<TagModel, dynamic>(
                sql,
                new { tag.Id },
                _connectionStringName)
                .FirstOrDefault();

            if (resultTag is not null)
            {
                continue;
            }

            output.Add(tag);
        }

        return output;
    }

    private void InsertTag(TagModel tag)
    {
        string sql = "";

        sql = @"INSERT INTO Tags (Title)
                VALUES (@Title);";

        _db.SqlExecute<dynamic>(
            sql,
            new { tag.Title },
            _connectionStringName);
    }

    public void DeleteEntry(EntryModel entry)
    {
        string sql = @"DELETE FROM Entries
                       WHERE Id = @Id;";

        _db.SqlExecute<dynamic>(sql, new { entry.Id }, _connectionStringName);

        sql = @"DELETE FROM EntriesTags
                WHERE EntryId = @Id;";

        _db.SqlExecute<dynamic>(sql, new { entry.Id }, _connectionStringName);

        DeleteUnusedTags(entry.Tags);
    }

    private void DeleteUnusedTags(IEnumerable<TagModel> tags)
    {
        if (tags.Any() == false)
        {
            return;
        }

        string sql = "";

        foreach (var tag in tags)
        {
            sql = @"SELECT TagId
                    FROM EntriesTags
                    WHERE TagId = @Id;";

            var result = _db.SqlQuery<int, dynamic>(
                sql,
                new { tag.Id },
                _connectionStringName);

            if (result.Count > 0)
            {
                break;
            }

            sql = @"DELETE FROM Tags
                    WHERE Id = @Id;";

            _db.SqlExecute<dynamic>(sql, new { tag.Id }, _connectionStringName);
        }
    }
}