using EncryptPad.Interfaces;

namespace EncryptPad.Shared.Models
{
    public class UnencryptedText : ITextModel
    {
        public string Text { get; set; } = string.Empty;
    }
}
