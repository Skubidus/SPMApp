using SPMLibrary.DataAccess;
using SPMLibrary.Models;

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
        if (id < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

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
        if (entryId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(entryId));
        }

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
        ArgumentNullException.ThrowIfNull(entry);

        throw new NotImplementedException();
    }
}