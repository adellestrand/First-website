using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriversJournal.Services
{
    /// <summary>Class that generate a random password </summary>
    public class CodeGenerator
    {

        /// <summary>
        /// Generated a random password
        /// </summary>
        /// <returns>string</returns>
    public string codeGenerator() {
    string lowers = "abcdefghijklmnopqrstuvwxyz";
    string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string number = "0123456789";

    Random random = new Random();

    string generated="";
    for (int i = 1; i <= 3; i++)
        generated = generated.Insert(
            random.Next(generated.Length), 
            lowers[random.Next(lowers.Length - 1)].ToString()
        );

    for (int i = 1; i <= 3; i++)
        generated = generated.Insert(
            random.Next(generated.Length), 
            uppers[random.Next(uppers.Length - 1)].ToString()
        );

    for (int i = 1; i <= 3; i++)
        generated = generated.Insert(
            random.Next(generated.Length), 
            number[random.Next(number.Length - 1)].ToString()
        );

    return generated;

}
    }
}