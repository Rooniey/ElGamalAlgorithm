using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using ElGamal;
using ElGamal.Model;
using ElGamal.Services;
using ElGamal.Services.Data;
using ElGamal.Services.Data.Padding;
using ElGamal.Services.Data.Sources;
using ElGamal.Services.Interfaces;
using Mvvm;

namespace Gui
{
    public class MainViewModel
    {
        private readonly ICryptoKeyGenerator _keyGenerator;

        private readonly ElGamalAlgorithm _algorithm;

        private IDataSource _dataSource;

        private IDataChunker _chunker;
        private IFileService _fileService;
        private IPaddingStrategy _paddingStrategy;

        public MainViewModel()
        {
            IRandomNumberProvider provider = new RandomNumberProvider();
            _keyGenerator = new CryptoKeyGenerator(provider);
            _algorithm = new ElGamalAlgorithm(provider);
            _chunker = new DataChunker(new SimplePaddingStrategy());
            _fileService = new FileService();

            _paddingStrategy = new PaddingWithZeroesStrategy();

            GenerateKeysCommand = new RelayCommand(GenerateKeys);
            EncryptCommand = new RelayCommand(Encrypt);
        }

        public PrivateKey PrivateKey { get; set; }

        public PublicKey PublicKey { get; set; }

        public ICommand GenerateKeysCommand { get; set; }

        public ICommand EncryptCommand { get; set; }

        public ICommand DecryptCommand { get; set; }

        public bool SaveToFile { get; set; }


        public string FilePath { get; set; }


        private void GenerateKeys()
        {
            PrivateKey = _keyGenerator.GeneratePrivateKey(1024);
            PublicKey = _keyGenerator.GeneratePublicKey(PrivateKey);
        }

        private void Encrypt()
        {
            GenerateKeys();

            _dataSource = new FileDataSource(FilePath);
            byte[] rawData = _dataSource.GetData();

            var chunkedData = _chunker.ChunkData(rawData, (PrivateKey.P.bitCount() / 8) - 1);

            ElGamalCiphertext[] encryptedData = new ElGamalCiphertext[chunkedData.Length];
            for (int i = 0; i < chunkedData.Length; i++)
            {
                encryptedData[i] = _algorithm.Encrypt(chunkedData[i], PublicKey);
            }

            var bytes = _chunker.CiphertextsToBytes(encryptedData, (PrivateKey.P.bitCount() / 8));
            File.WriteAllBytes("fajne.txt", bytes);


            
            byte[] encryptedData2 = File.ReadAllBytes("fajne.txt");

            ElGamalCiphertext[] ciphertext = _chunker.BytesToCipherText(encryptedData2, PrivateKey.P.bitCount() / 8);
            BigInteger[] messages = new BigInteger[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                messages[i] = _algorithm.Decrypt(ciphertext[i], PrivateKey);
            }

            byte[] result = new byte[messages.Length * ((PrivateKey.P.bitCount() / 8) - 1)];

            for (int i = 0; i < messages.Length; i++)
            {
                byte[] block = new byte[(PrivateKey.P.bitCount() / 8) - 1];
                byte[] padded =
                    _paddingStrategy.ApplyPadding(messages[i].getBytes(), (PrivateKey.P.bitCount() / 8) - 1);
                padded.CopyTo(result, i * ((PrivateKey.P.bitCount() / 8) - 1));
            }
        }

        public void Decrypt()
        {
            _dataSource = new FileDataSource("fajne.txt");
            byte[] encryptedData = _dataSource.GetData();

            ElGamalCiphertext[] ciphertext = _chunker.BytesToCipherText(encryptedData, PrivateKey.P.bitCount() / 8);
            BigInteger[] messages = new BigInteger[ciphertext.Length/2];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                messages[i] = _algorithm.Decrypt(ciphertext[i], PrivateKey);
            }

            byte[] result = new byte[messages.Length * (PrivateKey.P.bitCount() / 8) - 1];

            for (int i = 0; i < messages.Length; i++)
            {
                _paddingStrategy.ApplyPadding(messages[i].getBytes(), (PrivateKey.P.bitCount() / 8) - 1).CopyTo(result, i * (PrivateKey.P.bitCount() / 8) - 1);
            }




        }

        private void SaveFile(ElGamalCiphertext[] ciphertext)
        {
            var bytes = _chunker.CiphertextsToBytes(ciphertext, (PrivateKey.P.bitCount()/8));
            _fileService.SaveToFile(bytes, "fajne.txt");
        }

    }
}