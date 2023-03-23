using EncryptPad.Interfaces;

namespace EncryptPad.Shared.Models
{
    public class EncryptedText : ITextModel
    {
        public string Text { get; set; } = string.Empty;
    }
}
