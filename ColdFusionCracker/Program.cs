using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ColdFusionCracker
{
    class Program
    {
        const int _MinPasswordLength = 9;
        const string _DomainFilter = "@google";
        const bool _IdentifySpecialCharacters = true;
        const string _SpecialCharactersRegex = "[!@#$%^&*():\"<>?{ }|,./;'\\-']";
        
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                FancyStuff.Header();
                Console.WriteLine("Please specify a text file with CSV contents in this format:");
                Console.WriteLine("email,passwordencrypted,passwordhash");
                Console.WriteLine("example:");
                Console.WriteLine("test1@google.com,Ydn0C8I2DPQvIW/jt++HlA==,zUAdEye+wyET5AYAlegMGw==");
            }

            FileInfo fi = new FileInfo(args[0]);
            if (!fi.Exists)
            {
                Console.WriteLine("File does not exist: " + fi.FullName);
                
            }
            else
            {
                Decrypt(fi);
            }

        }
        private static void Decrypt(FileInfo fi)
        {
            var usersList = ConvertCSVToList(fi.FullName, new char[] { ',', '|' });
            using StreamWriter file = new(fi.Name.Split('.')[0] + "Decrypted.txt", append: true);
            FancyStuff.Header();
            FancyStuff.DataHeader();
            int count = 0;
            foreach (UserPasswords up in usersList)
            {
                bool passChecks = false;
                if (PasswordLengthCheck(_MinPasswordLength, up.PasswordDecrpted))
                {
                    passChecks = true;
                }
                if (DomainFilterCheck())
                {
                    passChecks = true;
                }
                if (_IdentifySpecialCharacters)
                {
                    Regex r = new Regex(_SpecialCharactersRegex);
                    if (r.IsMatch(up.PasswordDecrpted))
                    {
                        passChecks = true;
                    }
                    else
                        passChecks = false;
                }
                if (passChecks)
                {
                    Console.WriteLine($"{up.Email.PadRight(47)} {up.PasswordDecrpted}");
                    count++;
                    file.WriteLine($"{up.Email},{up.PasswordDecrpted}");
                }
                
            }
            FancyStuff.Footer();
            Console.WriteLine($"{usersList.Count} Passwords decrypted.");
            Console.WriteLine($"{count} Passwords met requirements.");
            Console.WriteLine("\r\nPress any key to close...");
            Console.ReadKey();
        }


        private static bool ContainsDomain(string password)
        {
            return password.ToLower().Contains(_DomainFilter);
        }
        private static bool DomainFilterCheck()
        {
            return _DomainFilter.Length > 0;
        }
        private static bool PasswordLengthCheck(int length, string password)
        {
            if (password.Length > length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Converts to StronglyTypes List
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="splitChars"></param>
        /// <returns></returns>
        private static List<UserPasswords> ConvertCSVToList(string fileName, char[] splitChars)
        {
            List<UserPasswords> users = new List<UserPasswords>();
            using var streamReader = File.OpenText(fileName);
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false,
                MissingFieldFound = null
            };
            using var csvReader = new CsvReader(streamReader, csvConfig);
            var usersImport = csvReader.GetRecords<UserPasswords>();

            users.AddRange(usersImport);

            return users;
        }



    }
}
