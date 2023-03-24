(function DecryptPage() {

    const D = document;

    // Encrypt Text
    D.getElementById('decrypt-btn').addEventListener('click', async function (e) {
        e.preventDefault();

        let unencryptedtext = D.getElementById('encrypted-text').value;
        let url = '/Decrypt/DecryptText';
        let data = { text: unencryptedtext };

        try {
            let response = await axios.post(url, data);
            D.getElementById('decrypted-text').value = response.data;
        } catch (e) {
            console.log(e.message);
        }

    });

    // Copy Encrypted Text
    D.getElementById('copy-btn').addEventListener('click', async function (e) {
        e.preventDefault();

        let value = D.getElementById('decrypted-text').value;

        navigator.clipboard.writeText(value);
    });
})();