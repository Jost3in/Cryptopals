using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Cryptopals
{
    public static class Challange
    {
        static byte[] Hex2Bytes(string hex){

            List<byte> decBytes = new List<byte>();

            for (int i = 0; i < hex.Length-1; i+=2)
            {
                string hexByte = hex.Substring(i, 2);
                decBytes.Add(Convert.ToByte(hexByte, 16));
            }
            var decArray = decBytes.ToArray();
            return decArray;
        }
        static string BytesToString(byte[] inBytes){
            string outString = "";
            foreach (var inByte in inBytes)
            {
                outString += (char)inByte;
            }
            return outString;
        }
        static string BytesToBits(byte[] inBytes){
            string bitString = "";
            foreach (var inByte in inBytes)
            {
                bitString += Convert.ToString(inByte, 2).PadLeft(8,'0');
            }
            return bitString;
        }
        static string Xor(string inBits, string xorBits){
            int repeat = inBits.Length/xorBits.Length;
            string fullXor = string.Concat(Enumerable.Repeat(xorBits, repeat+1));
            string xorResult = "";
            for (int i = 0; i < inBits.Length; i++)
            {
                int a = inBits[i] == '1' ? 1 : 0;
                int b = fullXor[i] == '1' ? 1 : 0;
                xorResult += ((a+b)-(2*a*b)).ToString();
            }
            return xorResult;
        }
        static string BitsToHex(string bits){
            string hexOut ="";
            for (int i = 0; i < bits.Length; i+=8)
            {
                string byteString = bits.Substring(i, 8);
                hexOut += Convert.ToString(Convert.ToByte(byteString, 2), 16).PadLeft(2, '0');
            }
            return hexOut;
        }
        
        static double HammingDist(byte[] input, int testLength){
            // byte[] byteStr = Convert.FromBase64String(input);

            byte[] p1 = input[0..(testLength-1)];
            byte[] p2 = input[(testLength)..((testLength*2)-1)];
            string bits1 = "";
            string bits2 = "";
            int hammingDist = 0;
            double normHammingDist = 0.0;


            foreach (var item in p1)
            {
                byte[] byteValue = {(byte)item};
                bits1 += BytesToBits(byteValue);
            }
            foreach (var item in p2)
            {
                byte[] byteValue = {(byte)item};
                bits2 += BytesToBits(byteValue);
            }
            for (int i = 0; i < bits1.Length; i++)
            {
                hammingDist += bits1[i]!=bits2[i] ? 1 : 0;
                
            }
            normHammingDist = (double)hammingDist/bits1.Length;
            // normHammingDist = (double)hammingDist/testLength;

            // System.Console.WriteLine(hammingDist);
            return normHammingDist;
        }

        public static void Challange1(string hex){

            System.Console.WriteLine("\n***CHALLANGE 1**\n");
            System.Console.WriteLine(Convert.ToBase64String(Hex2Bytes(hex)));
            System.Console.WriteLine(BytesToString(Hex2Bytes(hex)));
        }
        public static void Challange2(string hex, string xorHex){
            System.Console.WriteLine("\n***CHALLANGE 2**\n");
            string inBits = BytesToBits(Hex2Bytes(hex));
            string inXorBits = BytesToBits(Hex2Bytes(xorHex));
            
            string xorString = Xor(inBits, inXorBits);

            System.Console.WriteLine(BytesToString(Hex2Bytes(xorHex)));
            System.Console.WriteLine(BytesToString(Hex2Bytes(BitsToHex(xorString))));

        }

        public static void Challange3(string inHex){
            System.Console.WriteLine("\n***CHALLANGE 3**\n");

            string inBits = BytesToBits(Hex2Bytes(inHex));
            string xorChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string commonChars = "EATNOISetanois ";
            string decryptRes = "";
            char decryptChar = '0';
            int bestScore = 0;  
            int score = 0;

            foreach(var character in xorChars)
            {
                byte[] byteChar = {(byte)character};
                string charBit = BytesToBits(byteChar);
                // System.Console.WriteLine(charBit.Length);
                string xOrResult = Xor(inBits, charBit);
                string result = BytesToString(Hex2Bytes(BitsToHex(xOrResult)));
                foreach (var cchar in commonChars)
                {
                    score += result.Count(c => c == cchar);
                }
                if(bestScore < score){
                    bestScore = score;
                    decryptRes = result;
                    decryptChar = character;
                }
                score = 0;
            }
        System.Console.WriteLine(decryptRes);
        System.Console.WriteLine($"Decrypt key: {decryptChar}");
        }

         public static void Challange4(){
            System.Console.WriteLine("\n***CHALLANGE 4**\n");

            string path = @"C:\Users\joste\Documents\Cryptopals\4.txt";

            string[] lines = File.ReadAllLines(path);           

            string xorChars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string commonChars = "EATNOISetanois ";
            string bestDecryptRes = "";
            string decryptRes = "";
            char decryptChar = '0';
            int bestLineScore = 0;
            int lineScore = 0;
            int overallScore = 0;

            foreach (var line in lines)
            {
                string inBits = BytesToBits(Hex2Bytes(line));

                foreach(var character in xorChars)
                {
                    byte[] byteChar = {(byte)character};
                    string charBit = BytesToBits(byteChar);
                    string xOrResult = Xor(inBits, charBit);
                    string result = BytesToString(Hex2Bytes(BitsToHex(xOrResult)));
                    foreach (var cchar in commonChars)
                    {
                        lineScore += result.Count(c => c == cchar);
                    }
                    if(bestLineScore < lineScore){
                        bestLineScore = lineScore;
                        decryptRes = result;
                        decryptChar = character;
                    }
                    lineScore = 0;
                }
                if(overallScore < bestLineScore){
                    overallScore = bestLineScore;
                    bestDecryptRes = decryptRes;
                }
                overallScore = 0;
                
            }
        System.Console.WriteLine(bestDecryptRes);
        System.Console.WriteLine($"Decrypt key: {decryptChar}");
        }

        public static void Challange5(string inString){
            System.Console.WriteLine(inString);

            string xOrString = "ICE";
            string xOrBits = "";
            List<byte> letterBytes = new List<byte>();
            // int counter = 0;
            // string xorResult = "";
            
            foreach (var letter in xOrString)
            {
                byte[] letterByte = {(byte)letter};
                string xLetterBits = BytesToBits(letterByte);
                xOrBits += xLetterBits;
            }

            foreach (var letter in inString)
            {
                byte[] letterByte = {(byte)letter};
                letterBytes.Add((byte)letter);
                System.Console.WriteLine(letter);
                System.Console.WriteLine((byte)letter);
                System.Console.WriteLine(BytesToBits(letterByte));
                System.Console.WriteLine(BitsToHex(BytesToBits(letterByte)));
            }

            byte[] letterByteArray = letterBytes.ToArray();
            string letterBits = BytesToBits(letterByteArray);

            string xorResult = Xor(letterBits, xOrBits);
            System.Console.WriteLine(BitsToHex(xorResult));
        }
        public static void Challange6(){
            string path = @"C:\Users\joste\Documents\Cryptopals\6.txt";

            string[] lines = File.ReadAllLines(path);
            string allString = "";
            // List<byte[]> byteLines = new List<byte[]>();
            double normHammingDist = 0.0;
            Dictionary<int, double> results = new Dictionary<int, double>();
            Dictionary<int, int> byteFrequ = new Dictionary<int, int>();


            foreach (string line in lines)
            {
                allString += line;
            }
           
            byte[] byteStr = Convert.FromBase64String(allString);

            for (int i = 2; i < 40; i++)
            {
                for (int j = 1; j < lines.Length; j+=2)
                {
                    byte[] hamStr = Convert.FromBase64String(lines[j-1]+lines[j]);
                    normHammingDist += HammingDist(hamStr, i);
                }
                normHammingDist /= (lines.Length/2);
                results.Add(i, normHammingDist);
            }

            var sorted = results.OrderBy(a => a.Value);
            var valArray = sorted.ToArray();
            var topRestults = valArray[0..3];

            //WRITE OUT KEYLENGTH POSSIBILITIES
            foreach (var result in topRestults)
            {
                System.Console.WriteLine(result.Key);
            }

            
            List<byte> blockByte = new List<byte>();
            Dictionary<int, byte[]> blocks = new Dictionary<int, byte[]>();

            string commonChars = "ETAOIN SHRDLUetaoinshrdlu";


            string decryptRes = "";
            char decryptChar = '0';
            int bestScore = 0;
            int score = 0;
            // byteStr = Convert.FromBase64String(allString);
            string resultKey = "";

            //SET KEY LENGTH
            int keyLength = 29;

            for (int j = 0; j < keyLength; j++)
            {
                for (int i = j; i < byteStr.Length; i+=keyLength)
                {
                    blockByte.Add(byteStr[i]);
                }
                blocks.Add(j, blockByte.ToArray());
                blockByte.Clear();
            }
            
            
            foreach (var item in blocks)
            {
                string inBits = BytesToBits(item.Value);

                for (int i = 0; i < 255; i++)
                {                
                    byte[] byteChar = {(byte)i};
                    string charBit = BytesToBits(byteChar);
                    string xOrResult = Xor(inBits, charBit);
                    string result = BytesToString(Hex2Bytes(BitsToHex(xOrResult)));
                    foreach (var cchar in commonChars)
                    {
                        score += result.Count(c => c == cchar);
                    }
                    if(bestScore < score){
                        bestScore = score;
                        decryptRes = result;
                        decryptChar = (char)i;
                    }
                    score = 0;
                }
                // System.Console.WriteLine(decryptRes);
                System.Console.WriteLine($"Decrypt key for {item.Key}: {decryptChar}");
                resultKey += decryptChar.ToString();
                decryptRes="";
                decryptChar='0';
                bestScore=0;
                score=0;
                // System.Console.WriteLine($"Decrypt key: {decryptChar}");
            }

            System.Console.WriteLine("Key: " + resultKey);
            
            List<byte> letterBytes = new List<byte>();


            foreach (var letter in resultKey)
            {
                byte[] letterByte = {(byte)letter};
                letterBytes.Add((byte)letter);
            }
            byte[] letterByteArray = letterBytes.ToArray();
            string letterBits = BytesToBits(letterByteArray);

            // string testLine = lines[0];
            // byte[] testBytes = Convert.FromBase64String(testLine);
            
            string textBits = BytesToBits(byteStr);
            string decyptAllBits = Xor(textBits, letterBits);
            string decryptAllString = BytesToString(Hex2Bytes(BitsToHex(decyptAllBits)));

            System.Console.WriteLine(decryptAllString);
            
        }
    }
}