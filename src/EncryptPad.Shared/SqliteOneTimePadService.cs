using EncryptPad.Shared.Models;
using SQLite;
using TextEncryption;

namespace EncryptPad.Shared
{
    public class SqliteOneTimePadService
    {

        private readonly OneTimePad _onetimepad;
        private readonly SQLiteAsyncConnection _sqliteAsyncConnection;

        public SqliteOneTimePadService(OneTimePad onetimepad, SQLiteAsyncConnection sqliteAsyncConnection)
        {
            _onetimepad = onetimepad;
            _sqliteAsyncConnection = sqliteAsyncConnection;
        }

        public async Task<string> EncryptTextAsync(UnencryptedText unencryptedText)
        {
            var key = _onetimepad.GenerateKey(unencryptedText.Text.Length);
            string? encryptedText = _onetimepad.Encrypt(unencryptedText.Text, key);
            var guid = Guid.NewGuid();
            var otpKey = new OTPKey() { Id = guid, Key = key, Date = DateTime.Now };

            await _sqliteAsyncConnection.InsertAsync(otpKey);

            var payload = otpKey.Id.ToString("N") + encryptedText;

            return payload;
        }

        public async Task<string> DecryptTextAsync(EncryptedText encryptedText)
        {
            var guid = encryptedText.Text.Substring(0, 32);
            Guid capturedGuid = new Guid(guid);


            var query = _sqliteAsyncConnection.Table<OTPKey>().Where(x => x.Id == capturedGuid);
            var result = await query.ToListAsync();
            OTPKey key = result[0];

            var otp = new OneTimePad();
            var test = encryptedText.Text.Substring(32);
            var decryptedText = otp.Decrypt(test, key.Key);

            // Delete from DB
            await _sqliteAsyncConnection.DeleteAsync<OTPKey>(key.Id);

            return decryptedText;
        }
    }
}