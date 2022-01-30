namespace LabPro.Web.Models
{
    public abstract class DatabaseEntity<T> : DatabaseEntity
    {
        public T Id { get; set; }
    }

    public abstract class DatabaseEntity
    {
        public DatabaseEntityStatus Status { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }

    public enum DatabaseEntityStatus
    {
        Active,
        Deleted
    }
}
