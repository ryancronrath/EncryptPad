using SQLite;

namespace EncryptPad.Shared.Models
{
    public class OTPKey
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;

    }
}
