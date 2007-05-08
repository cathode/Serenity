/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    public static class HexEncoding
    {
        #region Methods - Public
        /// <summary>
        /// Returns an array of bytes representing the hex input.
        /// </summary>
        /// <param name="input">The hex input as a char array.</param>
        /// <returns>An array of bytes representing the hex input.</returns>
        public static Byte[] Convert(params char[] input)
        {
            Byte[] output = new Byte[input.Length / 2];
            int n = 0;
            for (int i = 0; i < input.Length / 2; i++)
            {
                string Temp = new string(input, n, 2);
                output[i] = Byte.Parse(Temp, System.Globalization.NumberStyles.HexNumber);
                n += 2;
            }
            return output;
        }
        /// <summary>
        /// Returns an array of bytes representing the hex input.
        /// </summary>
        /// <param name="input">The hex input as a string.</param>
        /// <returns>An array of bytes representing the hex input.</returns>
        public static Byte[] Convert(string input)
        {
            return HexEncoding.Convert(input.ToCharArray());
        }
        public static string Convert(params Byte[] input)
        {
            StringBuilder result = new StringBuilder();
            foreach (Byte b in input)
            {
                result.Append(b.ToString("X2"));
            }
            return result.ToString();
        }
        public static string Convert(Byte input)
        {
            return input.ToString("X2");
        }
        public static bool Validate(char input)
        {
            switch (input)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                    return true;
                default:
                    return false;
            }
        }
        public static bool Validate(char[] input)
        {
            foreach (char c in input)
            {
                if (HexEncoding.Validate(c) == false)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool Validate(string input)
        {
            return HexEncoding.Validate(input.ToCharArray());
        }
        #endregion
    }
}
