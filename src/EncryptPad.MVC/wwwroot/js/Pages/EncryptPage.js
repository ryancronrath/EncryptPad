(function EncryptPage() {

    const D = document;

    // Encrypt Text
    D.getElementById('encrypt-btn').addEventListener('click', async function (e) {
        e.preventDefault();

        let unencryptedtext = D.getElementById('unencrypted-text').value;
        let url = '/Encrypt/EncryptText';
        let data = { text: unencryptedtext };

        try {
            let response = await axios.post(url, data);
            D.getElementById('encrypted-text').value = response.data;
        } catch (e) {
            console.log(e.message);
        }

    });

    // Copy Encrypted Text
    D.getElementById('copy-btn').addEventListener('click', async function (e) {
        e.preventDefault();

        let value = D.getElementById('encrypted-text').value;

        navigator.clipboard.writeText(value);

        D.getElementById('success-msg').classList.remove('hide');

    });
})();