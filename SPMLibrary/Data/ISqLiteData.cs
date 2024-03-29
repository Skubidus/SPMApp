﻿using SPMLibrary.Models;

namespace SPMLibrary.Data;
public interface ISqLiteData
{
    List<EntryModel> GetAllEntries();
    EntryModel? GetEntryById(int id);
    void InsertEntry(EntryModel entry);
    void UpdateEntry(EntryModel entry);
    void DeleteEntry(EntryModel entry);
}