using DriversJournal.Models;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace DriversJournal.Services
{
    /// <summary> 
    /// PasswordHasher is a static class that i used to create salt and hash of an password.
    /// As well as validating passwords. 
    /// </summary>
    public class PasswordHasher
    {


        /// <summary> DriversJournalContext db: The connection to the database. </summary>
        private static DriversJournalContext db = new DriversJournalContext();
        /// <summary> </summary>
        public const int SALT_BYTE_SIZE = 24;
        /// <summary>SALT_BYTE_SIZE and HASH_BYTE_SIZE: Size of the byte array that will be made during salting and hashing. </summary>
        public const int HASH_BYTE_SIZE = 24;
        /// <summary>PBKDF2_ITERATIONS: How many diffrent kinds of hashes there can be of an password. </summary>
        public const int PBKDF2_ITERATIONS = 1000;

        /// <summary>
        /// Method that creates a hashed password.
        /// Also creates salt based on the password. 
        /// </summary>
        /// <param name="userid">Id of an user</param>
        /// <param name="pass">Password of an user</param>
        /// <returns>Hashed Password</returns>
        public static string createHash(int userid, string pass)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            byte[] hash = pbkdf2(pass, salt);

            string saltedPass = Convert.ToBase64String(salt);
            string hashedPass = Convert.ToBase64String(hash);

            saveSalt(userid, saltedPass);

            return hashedPass;
        }

        /// <summary>
        /// Method for checking of a password is valid.
        /// </summary>
        /// <param name="userid">Id of an user</param>
        /// <param name="pass">Password of an user</param>
        /// <param name="hashedPass">Hashed password of an user from the database</param>
        /// <returns>True or False depending if password is valid</returns>
        public static bool validatePassword(int userid, string pass, string hashedPass)
        {
            byte[] salt = Convert.FromBase64String(getSalt(userid));
            byte[] hash = Convert.FromBase64String(hashedPass);
            byte[] testHash = pbkdf2(pass, salt);

            return slowEquals(hash, testHash);
        }

        /// <summary>
        /// Method for creating a pbkdf2 of password and salt.
        /// </summary>
        /// <param name="pass">Password of an user</param>
        /// <param name="salt">The salt of an password</param>
        /// <returns>Byte array of pbkdf2 value</returns>
        private static byte[] pbkdf2(string pass, byte[] salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pass, salt);
            pbkdf2.IterationCount = PBKDF2_ITERATIONS;
            return pbkdf2.GetBytes(HASH_BYTE_SIZE);
        }

        /// <summary>
        /// Method for checking user password against password of database.
        /// </summary>
        /// <param name="a">Password of an user</param>
        /// <param name="b">Hashed password from database</param>
        /// <returns>True or False depending if both passwords are the same</returns>
        private static bool slowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Method for getting the salt from the database.
        /// </summary>
        /// <param name="userid">Id of an user</param>
        /// <returns>The users salt</returns>
        private static string getSalt(int userid)
        {
            var salt = db.Salts.Where(r => r.UserId == userid).FirstOrDefault();
            return salt.SaltValue;
        }

        /// <summary>
        /// Method for saving the salt in the database.
        /// </summary>
        /// <param name="userid">Id of an user</param>
        /// <param name="usersalt">Salt of an user</param>
        private static void saveSalt(int userid, string usersalt)
        {
            Salt salt = new Salt
            {
                UserId = userid,
                SaltValue = usersalt
            };
            db.Salts.Add(salt);
            db.SaveChanges();
        }
    }
}