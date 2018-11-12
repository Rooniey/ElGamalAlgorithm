using System.Collections.Generic;
using System.Windows.Input;
using ElGamal;
using ElGamal.Model;
using ElGamal.Services;
using ElGamal.Services.Data;
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

        public MainViewModel()
        {
            IRandomNumberProvider provider = new RandomNumberProvider();
            _keyGenerator = new CryptoKeyGenerator(provider);
            _algorithm = new ElGamalAlgorithm(provider);
            _chunker = new DataChunker();
            _fileService = new FileService();

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
            PrivateKey = _keyGenerator.GeneratePrivateKey(2048);
            PublicKey = _keyGenerator.GeneratePublicKey(PrivateKey);
        }

        private void Encrypt()
        {
            _dataSource = new FileDataSource(FilePath);
            var message = _dataSource.GetData();
            
            List<ElGamalCiphertext> encryptedData = new List<ElGamalCiphertext>();
            foreach (BigInteger chunk in _chunker.ChunkData(message, PrivateKey.P.bitCount() / 8))
            {
                var cipher = _algorithm.Encrypt(chunk, PublicKey);

            }

            if (SaveToFile)
            {
                SaveFile(encryptedData);
            }
            else
            {

            }
            
        }

        public void Decrypt()
        {
            _dataSource = new FileDataSource(FilePath);
            var message 

        }

        private void SaveFile(List<ElGamalCiphertext> ciphertext)
        {
            var bytes = _chunker.CiphertextsToBytes(ciphertext, PrivateKey.P.bitCount()/8);
            _fileService.SaveToFile(bytes, "fajne.txt");
        }

    }
}