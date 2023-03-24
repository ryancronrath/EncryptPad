using System.Text;

namespace TextEncryption
{
    public class OneTimePad
    {

        private readonly List<char> characters = new()
        {
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z',
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '`',
            '-',
            '=',
            '~',
            '!',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '*',
            '(',
            ')',
            '_',
            '+',
            '[',
            ']',
            '{',
            '}',
            '\\',
            '|',
            ',',
            '.',
            '/',
            '<',
            '>',
            '?',
            ';',
            '\'',
            ':',
            '"',
            ' '
        };

        private readonly int mod;

        public OneTimePad()
        {
            mod = characters.Count;
        }

        public string Encrypt(string text, string key)
        {
            text.Replace("\n", "");
            text.Replace("\t", "");
            text.Replace("\r", "");
            StringBuilder encryptedText = new();

            for (int i = 0; i < text.Length; i++)
            {
                var textIndex = characters.IndexOf(text[i]);
                var keyIndex = characters.IndexOf(key[i]);
                var sum = textIndex + keyIndex;
                var encryptedIndex = sum % mod;
                var encryptedValue = characters[encryptedIndex];
                encryptedText.Append(encryptedValue);

                //Console.WriteLine($"{unencryptedText[i]}, {key[i]}");
                //Console.WriteLine($"{textIndex}, {keyIndex}");
                //Console.WriteLine($"Sum of indexes: {sum}");
                //Console.WriteLine($"Encrypted Index with mod: {encryptedIndex}");
                //Console.WriteLine($"Encrypted value = {encryptedValue}");
                //Console.WriteLine("==================================");
            }

            return encryptedText.ToString();
        }

        public string Decrypt(string text, string key)
        {
            StringBuilder decryptedText = new();

            for (int i = 0; i < text.Length; i++)
            {
                var textIndex = characters.IndexOf(text[i]);
                var keyIndex = characters.IndexOf(key[i]);
                var sum = textIndex - keyIndex;
                var encryptedIndex = sum % mod;

                // If negative add mod value to encryptedIndex to get actual index.
                encryptedIndex = (encryptedIndex < 0) ? (encryptedIndex + mod) : encryptedIndex;
                var encryptedValue = characters[encryptedIndex];
                decryptedText.Append(encryptedValue);

                //Console.WriteLine($"{encryptedText[i]}, {key[i]}");
                //Console.WriteLine($"{textIndex}, {keyIndex}");
                //Console.WriteLine($"Sum of indexes: {sum}");
                //Console.WriteLine($"Encrypted Index with mod: {encryptedIndex}");
                //Console.WriteLine($"Encrypted value = {encryptedValue}");
                //Console.WriteLine("==================================");
            }
            return decryptedText.ToString();
        }

        public string GenerateKey(int length)
        {
            Random r = new();
            StringBuilder key = new();
            for (int i = 0; i < length; i++)
            {
                key.Append(characters[r.Next(0, characters.Count)]);
            }
            return key.ToString();
        }
    }
}