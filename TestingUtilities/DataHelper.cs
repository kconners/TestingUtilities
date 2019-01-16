using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingUtilities
{
    public class DataHelper
    {
        private readonly Random _rng = new Random();

        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz1234567980";
        private const string _charsNoSpace = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567980";
        private const string _AlphaOnlyCharsNoSpace = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string _AlphaOnlyCharsWithSpace = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz";

        public double GetRandomNumber(double minimum, double maximum, int RoundToNumOfDecimals)
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, RoundToNumOfDecimals);
        }

        public string RandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
        public string RandomString(int size, int SpacesOrNo)
        {
            char[] buffer = new char[size];
            if (SpacesOrNo == 1)
            {
                for (int i = 0; i < size; i++)
                {
                    buffer[i] = _chars[_rng.Next(_chars.Length)];
                }
            }
            else if (SpacesOrNo == 0)
            {
                for (int i = 0; i < size; i++)
                {
                    buffer[i] = _charsNoSpace[_rng.Next(_charsNoSpace.Length)];
                }
            }
            return new string(buffer);
        }
        public string RandomString(int size, int SpacesOrNo, int NumericOrNo)
        {
            char[] buffer = new char[size];


            if (SpacesOrNo == 1)
            {
                if (NumericOrNo == 1)
                {
                    for (int i = 0; i < size; i++)
                    {
                        buffer[i] = _chars[_rng.Next(_chars.Length)];
                    }
                }
                else if (NumericOrNo == 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        buffer[i] = _AlphaOnlyCharsWithSpace[_rng.Next(_AlphaOnlyCharsWithSpace.Length)];
                    }
                }
            }
            else if (SpacesOrNo == 0)
            {
                if (NumericOrNo == 1)
                {
                    for (int i = 0; i < size; i++)
                    {
                        buffer[i] = _charsNoSpace[_rng.Next(_charsNoSpace.Length)];
                    }
                }
                else if (NumericOrNo == 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        buffer[i] = _AlphaOnlyCharsNoSpace[_rng.Next(_AlphaOnlyCharsNoSpace.Length)];
                    }
                }
            }
            return new string(buffer);
        }
        public string RandomStringFromAList(String ListOfStrings, string Delimiter)
        {
            char[] delimiters = new char[] { Convert.ToChar(Delimiter) };
            String[] List = ListOfStrings.Split(delimiters);

            int r = _rng.Next(List.Count());
            return (string)List[r];
        }
        public string RandomEmail(int length, int parts)
        {
            string Email = "";
            int part1, part2, part3, part4, part5;

            if (parts == 3)
            {

                part1 = Convert.ToInt16(GetRandomNumber(0, length - 7, 0));
                part2 = (length - 7 - part1);
                part3 = 5;
                Email = RandomString(part1, 0) + "@" + RandomString(part2, 0) + "." + RandomString(part3, 0);
            }

            return Email;
        }
    }
}
