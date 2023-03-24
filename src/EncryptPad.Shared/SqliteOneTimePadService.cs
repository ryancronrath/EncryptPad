using EncryptPad.Shared.Models;
using SQLite;
using TextEncryption;

namespace EncryptPad.Shared
{
    public class SqliteOneTimePadService
    {
        private readonly OneTimePad _onetimepad;
        private readonly SQLiteAsyncConnection _sqliteAsyncConnection;
        private readonly int _expirationInMinutes;

        public SqliteOneTimePadService(OneTimePad onetimepad, SQLiteAsyncConnection sqliteAsyncConnection, int expirationInMinutes)
        {
            _onetimepad = onetimepad;
            _sqliteAsyncConnection = sqliteAsyncConnection;
            _expirationInMinutes = expirationInMinutes;
        }

        public async Task<string> EncryptTextAsync(UnencryptedText unencryptedText)
        {
            var key = _onetimepad.GenerateKey(unencryptedText.Text.Length);
            string? encryptedText = _onetimepad.Encrypt(unencryptedText.Text, key);
            var guid = Guid.NewGuid();
            var otpKey = new OTPKey() { Id = guid, Key = key, Date = DateTime.Now };

            await CheckForTable();
            await _sqliteAsyncConnection.InsertAsync(otpKey);
            await Cleanup();
            var encryptedTextWithKey = otpKey.Id.ToString("N") + encryptedText;

            return encryptedTextWithKey;
        }

        public async Task<string> DecryptTextAsync(EncryptedText encryptedText)
        {
            var guid = encryptedText.Text[..32];
            Guid capturedGuid = new(guid);

            await CheckForTable();

            var query = _sqliteAsyncConnection.Table<OTPKey>().Where(x => x.Id == capturedGuid);
            var result = await query.ToListAsync();
            OTPKey key = result[0];

            var otp = new OneTimePad();
            var test = encryptedText.Text[32..];
            var decryptedText = otp.Decrypt(test, key.Key);

            // Delete from DB
            await _sqliteAsyncConnection.DeleteAsync<OTPKey>(key.Id);

            await Cleanup();

            return decryptedText;
        }

        /// <summary>
        /// Check for OTPKey table in sqlite database.  If not found a table will be created.
        /// </summary>
        /// <returns></returns>
        private async Task CheckForTable()
        {
            var table = await _sqliteAsyncConnection.GetTableInfoAsync(nameof(OTPKey));
            if (table.Count == 0)
            {
                await _sqliteAsyncConnection.CreateTableAsync<OTPKey>();
            }
        }

        /// <summary>
        /// Removes expired keys from database table.
        /// </summary>
        /// <returns></returns>
        private async Task Cleanup()
        {
            var query = _sqliteAsyncConnection.Table<OTPKey>();
            var result = await query.ToListAsync();

            foreach (var key in result)
            {
                if (key.Date.AddMinutes(_expirationInMinutes) < DateTime.Now)
                {
                    await _sqliteAsyncConnection.DeleteAsync<OTPKey>(key.Id);
                }
            }
        }
    }
}