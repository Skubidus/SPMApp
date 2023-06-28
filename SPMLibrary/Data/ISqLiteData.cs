using SPMLibrary.Models;

namespace SPMLibrary.Data;
public interface ISqLiteData
{
    List<Entry> GetAllEntries();
    Entry? GetEntryById(int id);
    void InsertEntry(Entry entry);
}