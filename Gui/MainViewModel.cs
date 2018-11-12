using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElGamal;
using ElGamal.Model;
using ElGamal.Services;
using ElGamal.Services.Data.Sources;
using ElGamal.Services.Interfaces;
using Microsoft.Win32;
using Mvvm;
using static ElGamal.Helpers.TextUtility;

namespace Gui
{
    public class MainViewModel : BindableBase
    {
        private readonly ICryptoKeyGenerator _keyGenerator;

        private readonly ElGamalAlgorithm _algorithm;

        private IDataSource _dataSource;

        public MainViewModel()
        {
            IRandomNumberProvider provider = new RandomNumberProvider();
            _keyGenerator = new CryptoKeyGenerator(provider);
            _algorithm = new ElGamalAlgorithm(provider);

            GenerateKeysCommand = new RelayCommand(GenerateKeys);
            EncryptCommand = new RelayCommand(Encrypt, CanUseAlgorithm);
            DecryptCommand = new RelayCommand(Decrypt, CanUseAlgorithm);
            SelectFileCommand = new RelayCommand(SelectFile);
        }

        public int[] KeySizes { get; } = new int[] { 512, 1024, 2048 };

        public int SelectedKeySize { get; set; } = 1024;

        public PrivateKey PrivateKey { get; set; }

        public PublicKey PublicKey { get; set; }

        public RelayCommand GenerateKeysCommand { get; set; }

        public RelayCommand EncryptCommand { get; set; }

        public RelayCommand DecryptCommand { get; set; }

        private bool _isFileEncryption;

        public bool IsFileEncryption
        {
            get => _isFileEncryption;
            set
            {
                SetProperty(ref _isFileEncryption, value);
                EncryptCommand.RaiseCanExecuteChanged();
                DecryptCommand.RaiseCanExecuteChanged();
            }
        }

        #region DATA SOURCE

        public RelayCommand SelectFileCommand { get; set; }

        private void SelectFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a file to encrypt";

            if (fileDialog.ShowDialog() == true)
            {
                FilePath = fileDialog.FileName;
            }
        }

        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            set
            {
                SetProperty(ref _filePath, value);
                EncryptCommand.RaiseCanExecuteChanged();
                DecryptCommand.RaiseCanExecuteChanged();
            }
        }

        public List<Encoding> Encodings => Enum.GetValues(typeof(Encoding)).OfType<Encoding>().ToList();

        private string _textSource;
        public string TextSource
        {
            get => _textSource;
            set
            {
                _textSource = value;
                EncryptCommand.RaiseCanExecuteChanged();
                DecryptCommand.RaiseCanExecuteChanged();
            }
        }

        public Encoding SelectedEncoding { get; set; }

        #endregion

        private void CreateDataSource()
        {
            if (IsFileEncryption)
            {
                _dataSource = new FileDataSource(FilePath);
            }
            else
            {
                _dataSource = new TextDataSource(TextSource, SelectedEncoding);
            }
        }

        private void GenerateKeys()
        {
            PrivateKey = _keyGenerator.GeneratePrivateKey(SelectedKeySize);
            PublicKey = _keyGenerator.GeneratePublicKey(PrivateKey);
            EncryptCommand.RaiseCanExecuteChanged();
            DecryptCommand.RaiseCanExecuteChanged();
        }

        private bool CanUseAlgorithm()
        {
            bool isDataSource = (IsFileEncryption && File.Exists(FilePath)) ||
                                (!IsFileEncryption && !string.IsNullOrEmpty(TextSource));
            return isDataSource && PrivateKey != null && PublicKey != null;
        }

        private void Encrypt()
        {
            CreateDataSource();
            byte[] rawData = _dataSource.GetData();
            byte[] encryptedData = _algorithm.Encrypt(rawData, PublicKey);
            File.WriteAllBytes("test-encrypted.txt", encryptedData);
        }

        private void Decrypt()
        {
            CreateDataSource();
            _dataSource = new FileDataSource("test-encrypted.txt");
            byte[] encryptedData = _dataSource.GetData();
            byte[] decryptedData = _algorithm.Decrypt(encryptedData, PrivateKey);
            File.WriteAllBytes("test-decrypted.txt", decryptedData);
        }


    }
}