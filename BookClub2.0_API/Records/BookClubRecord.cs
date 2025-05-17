using BookClub2._0.Models;
namespace BookClub2._0_API.Records
{
    public record BookClubRecord (int id, string name, string description);
    public static class RecordhelperBookclubRecords
    {
        public static BookClub ConvertBookClubRecord(BookClubRecord record)
        {
            if (record.id == null) { throw new ArgumentNullException("Exception" + record.id); }
            if (string.IsNullOrEmpty(record.name)) { throw new ArgumentNullException(nameof(record.name), "Name cannot be null or empty."); }
            if (string.IsNullOrEmpty(record.description)) { throw new ArgumentNullException(nameof(record.description), "Description cannot be null or empty."); }
            if (record.description.Length > 50) { throw new ArgumentOutOfRangeException(nameof(record.description), "Description cannot exceed 50 characters."); }
            

            return new BookClub
            {
                Id = record.id,
                Name = record.name,
                Description = record.description,
                
            };
        }
    }


}
