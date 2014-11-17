//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using Org.BouncyCastle.Crypto;
//using Org.BouncyCastle.Crypto.Generators;
//using Org.BouncyCastle.Crypto.Parameters;
//using Org.BouncyCastle.Security;
//using Org.BouncyCastle.Math.EC;

//namespace BitcoinMeum
//{
//    class AdressGenerator
//    {


//        public class RIPEMD160
//        {

//            private static int[] ar1 = {11, 14, 15, 12, 5, 8, 7, 9, 11, 13, 14, 15, 6, 7, 9, 8,
//                                                                    7, 6, 8, 13, 11, 9, 7, 15, 7, 12, 15, 9, 11, 7, 13, 12,
//                                                                    11, 13, 6, 7, 14, 9, 13, 15, 14, 8, 13, 6, 5, 12, 7, 5,
//                                                                    11, 12, 14, 15, 14, 15, 9, 8, 9, 14, 5, 6, 8, 6, 5, 12,
//                                                                    9, 15, 5, 11, 6, 8, 13, 12, 5, 12, 13, 14, 11, 8, 5, 6};

//            private static int[] ar2 = {8, 9, 9, 11, 13, 15, 15, 5, 7, 7, 8, 11, 14, 14, 12, 6,
//                                                                    9, 13, 15, 7, 12, 8, 9, 11, 7, 7, 12, 7, 6, 15, 13, 11,
//                                                                    9, 7, 15, 11, 8, 6, 6, 14, 12, 13, 5, 14, 13, 13, 7, 5,
//                                                                    15, 5, 8, 11, 14, 14, 6, 14, 6, 9, 12, 9, 12, 5, 15, 8,
//                                                                        8, 5, 12, 9, 12, 5, 14, 6, 8, 13, 6, 5, 15, 13, 11, 11 };
//            private static int[][] ArgArray = { ar1, ar2 };

//            private static int[] iar1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
//                                                                            7, 4, 13, 1, 10, 6, 15, 3, 12, 0, 9, 5, 2, 14, 11, 8,
//                                                                        3, 10, 14, 4, 9, 15, 8, 1, 2, 7, 0, 6, 13, 11, 5, 12,
//                                                                        1, 9, 11, 10, 0, 8, 12, 4, 13, 3, 7, 15, 14, 5, 6, 2,
//                                                                        4, 0, 5, 9, 7, 12, 2, 10, 14, 1, 3, 8, 11, 6, 15, 13};
//            private static int[] iar2 = {5, 14, 7, 0, 9, 2, 11, 4, 13, 6, 15, 8, 1, 10, 3, 12,
//                                                                        6, 11, 3, 7, 0, 13, 5, 10, 14, 15, 8, 12, 4, 9, 1, 2,
//                                                                        15, 5, 1, 3, 7, 14, 6, 9, 11, 8, 12, 2, 10, 0, 4, 13,
//                                                                        8, 6, 4, 1, 3, 11, 15, 0, 5, 12, 2, 13, 9, 7, 10, 14,
//                                                                        12, 15, 10, 4, 1, 5, 8, 7, 6, 2, 13, 14, 0, 3, 9, 11 };
//            private static int[][] IndexArray = { iar1, iar2 };

//            private int[] MDbuf;

//            public RIPEMD160()
//            {
//                MDbuf = new int[5];
//                MDbuf[0] = 0x67452301;
//                MDbuf[1] = Int32.Parse("0xefcdab89");
//                MDbuf[2] = Int32.Parse("0x98badcfe");
//                MDbuf[3] = 0x10325476;
//                MDbuf[4] = Int32.Parse("0xc3d2e1f0");
//                working = new int[16];
//                working_ptr = 0;
//                msglen = 0;
//            }

//            public void reset()
//            {
//                MDbuf = new int[5];
//                MDbuf[0] = 0x67452301;
//                MDbuf[1] = Int32.Parse("0xefcdab89");
//                MDbuf[2] = Int32.Parse("0x98badcfe");
//                MDbuf[3] = 0x10325476;
//                MDbuf[4] = Int32.Parse("0xc3d2e1f0");
//                working = new int[16];
//                working_ptr = 0;
//                msglen = 0;
//            }

//            private void compress(int[] X)
//            {
//                int index = 0;

//                int a, b, c, d, e;
//                int A, B, C, D, E;
//                int temp, s;

//                A = a = MDbuf[0];
//                B = b = MDbuf[1];
//                C = c = MDbuf[2];
//                D = d = MDbuf[3];
//                E = e = MDbuf[4];

//                for (; index < 16; index++)
//                {
//                    // The 16 FF functions - round 1 */
//                    temp = a + (b ^ c ^ d) + X[IndexArray[0][index]];
//                    a = e;
//                    e = d;
//                    d = (c << 10) | (c >> 22);
//                    c = b;
//                    s = ArgArray[0][index];
//                    b = ((temp << s) | (temp >> (32 - s))) + a;

//                    // The 16 JJJ functions - parallel round 1 */
//                    temp = A + (B ^ (C | ~D)) + X[IndexArray[1][index]] + 0x50a28be6;
//                    A = E;
//                    E = D;
//                    D = (C << 10) | (C >> 22);
//                    C = B;
//                    s = ArgArray[1][index];
//                    B = ((temp << s) | (temp >> (32 - s))) + A;
//                }

//                for (; index < 32; index++)
//                {
//                    // The 16 GG functions - round 2 */
//                    temp = a + ((b & c) | (~b & d)) + X[IndexArray[0][index]] + 0x5a827999;
//                    a = e;
//                    e = d;
//                    d = (c << 10) | (c >> 22);
//                    c = b;
//                    s = ArgArray[0][index];
//                    b = ((temp << s) | (temp >> (32 - s))) + a;

//                    // The 16 III functions - parallel round 2 */
//                    temp = A + ((B & D) | (C & ~D)) + X[IndexArray[1][index]] + 0x5c4dd124;
//                    A = E;
//                    E = D;
//                    D = (C << 10) | (C >> 22);
//                    C = B;
//                    s = ArgArray[1][index];
//                    B = ((temp << s) | (temp >> (32 - s))) + A;
//                }

//                for (; index < 48; index++)
//                {
//                    // The 16 HH functions - round 3 */
//                    temp = a + ((b | ~c) ^ d) + X[IndexArray[0][index]] + 0x6ed9eba1;
//                    a = e;
//                    e = d;
//                    d = (c << 10) | (c >> 22);
//                    c = b;
//                    s = ArgArray[0][index];
//                    b = ((temp << s) | (temp >> (32 - s))) + a;

//                    // The 16 HHH functions - parallel round 3 */
//                    temp = A + ((B | ~C) ^ D) + X[IndexArray[1][index]] + 0x6d703ef3;
//                    A = E;
//                    E = D;
//                    D = (C << 10) | (C >> 22);
//                    C = B;
//                    s = ArgArray[1][index];
//                    B = ((temp << s) | (temp >> (32 - s))) + A;
//                }

//                for (; index < 64; index++)
//                {
//                    // The 16 II functions - round 4 */
//                    temp = a + ((b & d) | (c & ~d)) + X[IndexArray[0][index]] + Int32.Parse("0x8f1bbcdc");
//                    a = e;
//                    e = d;
//                    d = (c << 10) | (c >> 22);
//                    c = b;
//                    s = ArgArray[0][index];
//                    b = ((temp << s) | (temp >> (32 - s))) + a;

//                    // The 16 GGG functions - parallel round 4 */
//                    temp = A + ((B & C) | (~B & D)) + X[IndexArray[1][index]] + 0x7a6d76e9;
//                    A = E;
//                    E = D;
//                    D = (C << 10) | (C >> 22);
//                    C = B;
//                    s = ArgArray[1][index];
//                    B = ((temp << s) | (temp >> (32 - s))) + A;
//                }

//                for (; index < 80; index++)
//                {
//                    // The 16 JJ functions - round 5 */
//                    temp = a + (b ^ (c | ~d)) + X[IndexArray[0][index]] + Int32.Parse("0xa953fd4e");
//                    a = e;
//                    e = d;
//                    d = (c << 10) | (c >> 22);
//                    c = b;
//                    s = ArgArray[0][index];
//                    b = ((temp << s) | (temp >> (32 - s))) + a;

//                    // The 16 FFF functions - parallel round 5 */
//                    temp = A + (B ^ C ^ D) + X[IndexArray[1][index]];
//                    A = E;
//                    E = D;
//                    D = (C << 10) | (C >> 22);
//                    C = B;
//                    s = ArgArray[1][index];
//                    B = ((temp << s) | (temp >> (32 - s))) + A;
//                }

//                /* combine results */
//                D += c + MDbuf[1];               /* final result for MDbuf[0] */
//                MDbuf[1] = MDbuf[2] + d + E;
//                MDbuf[2] = MDbuf[3] + e + A;
//                MDbuf[3] = MDbuf[4] + a + B;
//                MDbuf[4] = MDbuf[0] + b + C;
//                MDbuf[0] = D;
//            }

//            private void MDfinish(int[] array, int lswlen, int mswlen)
//            {
//                int[] X = array;                                /* message words */

//                /* append the bit m_n == 1 */
//                X[(lswlen >> 2) & 15] ^= 1 << (((lswlen & 3) << 3) + 7);

//                if ((lswlen & 63) > 55)
//                {
//                    /* length goes to next block */
//                    compress(X);
//                    for (int i = 0; i < 14; i++)
//                    {
//                        X[i] = 0;
//                    }
//                }

//                /* append length in bits*/
//                X[14] = lswlen << 3;
//                X[15] = (lswlen >> 29) | (mswlen << 3);
//                compress(X);
//            }
//            private int[] working;
//            private int working_ptr;
//            private int msglen;

//            public void update(byte input)
//            {
//                working[working_ptr >> 2] ^= ((int)input) << ((working_ptr & 3) << 3);
//                working_ptr++;
//                if (working_ptr == 64)
//                {
//                    compress(working);
//                    for (int j = 0; j < 16; j++)
//                    {
//                        working[j] = 0;
//                    }
//                    working_ptr = 0;
//                }
//                msglen++;
//            }

//            public void update(byte[] input)
//            {
//                for (int i = 0; i < input.Length; i++)
//                {
//                    working[working_ptr >> 2] ^= ((int)input[i]) << ((working_ptr & 3) << 3);
//                    working_ptr++;
//                    if (working_ptr == 64)
//                    {
//                        compress(working);
//                        for (int j = 0; j < 16; j++)
//                        {
//                            working[j] = 0;
//                        }
//                        working_ptr = 0;
//                    }
//                }
//                msglen += input.Length;
//            }

//            public void update(byte[] input, int offset, int len)
//            {
//                if (offset + len >= input.Length)
//                {
//                    for (int i = offset; i < input.Length; i++)
//                    {
//                        working[working_ptr >> 2] ^= ((int)input[i]) << ((working_ptr & 3) << 3);
//                        working_ptr++;
//                        if (working_ptr == 64)
//                        {
//                            compress(working);
//                            for (int j = 0; j < 16; j++)
//                            {
//                                working[j] = 0;
//                            }
//                            working_ptr = 0;
//                        }
//                    }
//                    msglen += input.Length - offset;
//                }
//                else
//                {
//                    for (int i = offset; i < offset + len; i++)
//                    {
//                        working[working_ptr >> 2] ^= ((int)input[i]) << ((working_ptr & 3) << 3);
//                        working_ptr++;
//                        if (working_ptr == 64)
//                        {
//                            compress(working);
//                            for (int j = 0; j < 16; j++)
//                            {
//                                working[j] = 0;
//                            }
//                            working_ptr = 0;
//                        }
//                    }
//                    msglen += len;
//                }
//            }

//            public void update(String s)
//            {
//                byte[] bytearray = new byte[s.Length];
//                for (int i = 0; i < bytearray.Length; i++)
//                {
//                    bytearray[i] = (byte)s[i];
//                }
//                update(bytearray);
//            }

//            public byte[] digestBin()
//            {
//                MDfinish(working, msglen, 0);
//                byte[] res = new byte[20];
//                for (int i = 0; i < 20; i++)
//                {
//                    res[i] = (byte)((MDbuf[i >> 2] >> ((i & 3) << 3)) & 0x000000FF);
//                }
//                return res;
//            }

//            public byte[] digest(byte[] input)
//            {
//                update(input);
//                return digestBin();
//            }

//            public String digest()
//            {
//                MDfinish(working, msglen, 0);
//                byte[] res = new byte[20];
//                for (int i = 0; i < 20; i++)
//                {
//                    res[i] = (byte)((MDbuf[i >> 2] >> ((i & 3) << 3)) & 0x000000FF);
//                }

//                String hex = "";
//                for (int i = 0; i < res.Length; i++)
//                {
//                    hex += byteToHex(res[i]);
//                }
//                return hex;
//            }

//            public byte[] digest(byte[] input, int offset, int len)
//            {
//                update(input, offset, len);
//                return digestBin();
//            }

//            public int[] intdigest()
//            {
//                int[] res = new int[5];
//                for (int i = 0; i < 5; i++)
//                {
//                    res[i] = MDbuf[i];
//                }
//                return res;
//            }

//            public static String byteToHex(byte b)
//            {
//                byte top = (byte)(((256 + b) / 16) & 15);
//                byte bottom = (byte)((256 + b) & 15);

//                String res;

//                if (top > 9)
//                {
//                    res = "" + (char)('a' + (top - 10));
//                }
//                else
//                {
//                    res = "" + (char)('0' + top);
//                }
//                if (bottom > 9)
//                {
//                    res += (char)('a' + (bottom - 10));
//                }
//                else
//                {
//                    res += (char)('0' + bottom);
//                }
//                return res;
//            }

//            public static String RIPEMD160String(String txt)
//            {
//                RIPEMD160 r = new RIPEMD160();
//                r.update(txt);
//                return r.digest();

//            }
//        }
//        public class Address
//        {
//            private byte[] Base58ToByteArray(string base58)
//            {

//                Org.BouncyCastle.Math.BigInteger bi2 = new Org.BouncyCastle.Math.BigInteger("0");
//                string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";

//                bool IgnoreChecksum = false;

//                foreach (char c in base58)
//                {
//                    if (b58.IndexOf(c) != -1)
//                    {
//                        bi2 = bi2.Multiply(new Org.BouncyCastle.Math.BigInteger("58"));
//                        bi2 = bi2.Add(new Org.BouncyCastle.Math.BigInteger(b58.IndexOf(c).ToString()));
//                    }
//                    else if (c == '?')
//                    {
//                        IgnoreChecksum = true;
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }

//                byte[] bb = bi2.ToByteArrayUnsigned();

//                // interpret leading '1's as leading zero bytes
//                foreach (char c in base58)
//                {
//                    if (c != '1') break;
//                    byte[] bbb = new byte[bb.Length + 1];
//                    Array.Copy(bb, 0, bbb, 1, bb.Length);
//                    bb = bbb;
//                }

//                if (bb.Length < 4) return null;

//                if (IgnoreChecksum == false)
//                {

//                    SHA256Managed sha256 = new SHA256Managed();
//                    byte[] checksum = sha256.ComputeHash(bb, 0, bb.Length - 4);
//                    checksum = sha256.ComputeHash(checksum);
//                    for (int i = 0; i < 4; i++)
//                    {
//                        if (checksum[i] != bb[bb.Length - 4 + i]) return null;
//                    }
//                }

//                byte[] rv = new byte[bb.Length - 4];
//                Array.Copy(bb, 0, rv, 0, bb.Length - 4);
//                return rv;
//            }

//            private string ByteArrayToString(byte[] ba)
//            {
//                return ByteArrayToString(ba, 0, ba.Length);
//            }

//            private string ByteArrayToString(byte[] ba, int offset, int count)
//            {
//                string rv = "";
//                int usedcount = 0;
//                for (int i = offset; usedcount < count; i++, usedcount++)
//                {
//                    rv += String.Format("{0:X2}", ba[i]) + " ";
//                }
//                return rv;
//            }

//            private string ByteArrayToBase58(byte[] ba)
//            {
//                Org.BouncyCastle.Math.BigInteger addrremain = new Org.BouncyCastle.Math.BigInteger(1, ba);

//                Org.BouncyCastle.Math.BigInteger big0 = new Org.BouncyCastle.Math.BigInteger("0");
//                Org.BouncyCastle.Math.BigInteger big58 = new Org.BouncyCastle.Math.BigInteger("58");

//                string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

//                string rv = "";

//                while (addrremain.CompareTo(big0) > 0)
//                {
//                    int d = Convert.ToInt32(addrremain.Mod(big58).ToString());
//                    addrremain = addrremain.Divide(big58);
//                    rv = b58.Substring(d, 1) + rv;
//                }

//                // handle leading zeroes
//                foreach (byte b in ba)
//                {
//                    if (b != 0) break;
//                    rv = "1" + rv;

//                }
//                return rv;
//            }


//            private string ByteArrayToBase58Check(byte[] ba)
//            {

//                byte[] bb = new byte[ba.Length + 4];
//                Array.Copy(ba, bb, ba.Length);
//                SHA256Managed sha256 = new SHA256Managed();
//                byte[] thehash = sha256.ComputeHash(ba);
//                thehash = sha256.ComputeHash(thehash);
//                for (int i = 0; i < 4; i++) bb[ba.Length + i] = thehash[i];
//                return ByteArrayToBase58(bb);
//            }

//            private void button4_Click(object sender, EventArgs e)
//            {

//                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");


//            }

//            private byte[] GetHexBytes(string source, int minimum)
//            {
//                byte[] hex = GetHexBytes(source);
//                if (hex == null) return null;
//                // assume leading zeroes if we're short a few bytes
//                if (hex.Length > (minimum - 6) && hex.Length < minimum)
//                {
//                    byte[] hex2 = new byte[minimum];
//                    Array.Copy(hex, 0, hex2, minimum - hex.Length, hex.Length);
//                    hex = hex2;
//                }
//                // clip off one overhanging leading zero if present
//                if (hex.Length == minimum + 1 && hex[0] == 0)
//                {
//                    byte[] hex2 = new byte[minimum];
//                    Array.Copy(hex, 1, hex2, 0, minimum);
//                    hex = hex2;

//                }

//                return hex;
//            }


//            private byte[] GetHexBytes(string source)
//            {


//                List<byte> bytes = new List<byte>();
//                // copy s into ss, adding spaces between each byte
//                string s = source;
//                string ss = "";
//                int currentbytelength = 0;
//                foreach (char c in s.ToCharArray())
//                {
//                    if (c == ' ')
//                    {
//                        currentbytelength = 0;
//                    }
//                    else
//                    {
//                        currentbytelength++;
//                        if (currentbytelength == 3)
//                        {
//                            currentbytelength = 1;
//                            ss += ' ';
//                        }
//                    }
//                    ss += c;
//                }

//                foreach (string b in ss.Split(' '))
//                {
//                    int v = 0;
//                    if (b.Trim() == "") continue;
//                    foreach (char c in b.ToCharArray())
//                    {
//                        if (c >= '0' && c <= '9')
//                        {
//                            v *= 16;
//                            v += (c - '0');

//                        }
//                        else if (c >= 'a' && c <= 'f')
//                        {
//                            v *= 16;
//                            v += (c - 'a' + 10);
//                        }
//                        else if (c >= 'A' && c <= 'F')
//                        {
//                            v *= 16;
//                            v += (c - 'A' + 10);
//                        }

//                    }
//                    v &= 0xff;
//                    bytes.Add((byte)v);
//                }
//                return bytes.ToArray();
//            }

//            private byte[] ValidateAndGetHexPrivateKey(byte leadingbyte)
//            {
//                byte[] hex = GetHexBytes(txtPrivHex.Text, 32);

//                if (hex == null || hex.Length < 32 || hex.Length > 33)
//                {
//                    //MessageBox.Show("Hex is not 32 or 33 bytes.");
//                    return null;
//                }

//                // if leading 00, change it to 0x80
//                if (hex.Length == 33)
//                {
//                    if (hex[0] == 0 || hex[0] == 0x80)
//                    {
//                        hex[0] = 0x80;
//                    }
//                    else
//                    {
//                        //MessageBox.Show("Not a valid private key");
//                        return null;
//                    }
//                }

//                // add 0x80 byte if not present
//                if (hex.Length == 32)
//                {
//                    byte[] hex2 = new byte[33];
//                    Array.Copy(hex, 0, hex2, 1, 32);
//                    hex2[0] = 0x80;
//                    hex = hex2;
//                }

//                hex[0] = leadingbyte;
//                return hex;

//            }


//            private byte[] ValidateAndGetHexPublicKey()
//            {
//                byte[] hex = GetHexBytes(txtPubHex.Text, 64);

//                if (hex == null || hex.Length < 64 || hex.Length > 65)
//                {
//                    //MessageBox.Show("Hex is not 64 or 65 bytes.");
//                    return null;
//                }

//                // if leading 00, change it to 0x80
//                if (hex.Length == 65)
//                {
//                    if (hex[0] == 0 || hex[0] == 4)
//                    {
//                        hex[0] = 4;
//                    }
//                    else
//                    {
//                        //MessageBox.Show("Not a valid public key");
//                        return null;
//                    }
//                }

//                // add 0x80 byte if not present
//                if (hex.Length == 64)
//                {
//                    byte[] hex2 = new byte[65];
//                    Array.Copy(hex, 0, hex2, 1, 64);
//                    hex2[0] = 4;
//                    hex = hex2;
//                }
//                return hex;
//            }

//            private byte[] ValidateAndGetHexPublicHash()
//            {
//                byte[] hex = GetHexBytes(txtPubHash.Text, 20);

//                if (hex == null || hex.Length != 20)
//                {
//                    //MessageBox.Show("Hex is not 20 bytes.");
//                    return null;
//                }
//                return hex;
//            }


//            private void btnPrivHexToWIF_Click(object sender, EventArgs e)
//            {
//                byte[] hex = ValidateAndGetHexPrivateKey(0x80);
//                if (hex == null) return;
//                txtPrivWIF.Text = ByteArrayToBase58Check(hex);
//            }

//            private void btnPrivWIFToHex_Click(object sender, EventArgs e)
//            {
//                byte[] hex = Base58ToByteArray(txtPrivWIF.Text);
//                if (hex == null)
//                {
//                    int L = txtPrivWIF.Text.Length;
//                    if (L >= 50 && L <= 52)
//                    {
//                        if (MessageBox.Show("Private key is not valid.  Attempt to correct?", "Invalid address", MessageBoxButtons.YesNo) == DialogResult.Yes)
//                        {
//                            CorrectWIF();
//                            return;
//                        }
//                    }
//                    else
//                    {
//                        //MessageBox.Show("WIF private key is not valid.");
//                    }
//                    return;
//                }
//                if (hex.Length != 33)
//                {
//                    //MessageBox.Show("WIF private key is not valid (wrong byte count, should be 33, was " + hex.Length + ")");
//                    return;
//                }

//                txtPrivHex.Text = ByteArrayToString(hex, 1, 32);


//            }


//            private void btnPrivToPub_Click(object sender, EventArgs e)
//            {
//                byte[] hex = ValidateAndGetHexPrivateKey(0x00);
//                if (hex == null) return;
//                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
//                Org.BouncyCastle.Math.BigInteger Db = new Org.BouncyCastle.Math.BigInteger(hex);
//                ECPoint dd = ps.G.Multiply(Db);

//                byte[] pubaddr = new byte[65];
//                byte[] Y = dd.Y.ToBigInteger().ToByteArray();
//                Array.Copy(Y, 0, pubaddr, 64 - Y.Length + 1, Y.Length);
//                byte[] X = dd.X.ToBigInteger().ToByteArray();
//                Array.Copy(X, 0, pubaddr, 32 - X.Length + 1, X.Length);
//                pubaddr[0] = 4;

//                txtPubHex.Text = ByteArrayToString(pubaddr);

//            }


//            private void btnPubHexToHash_Click(object sender, EventArgs e)
//            {
//                byte[] hex = ValidateAndGetHexPublicKey();
//                if (hex == null) return;


//                SHA256Managed sha256 = new SHA256Managed();
//                byte[] shaofpubkey = sha256.ComputeHash(hex);
//                //RipeMD160Digest rip = new RipeMD160Digest();
//                //rip.Update(sha256);
//                RIPEMD160 rip = System.Security.Cryptography.RIPEMD160.Create();
//                byte[] ripofpubkey = rip.ComputeHash(shaofpubkey);

//                txtPubHash.Text = ByteArrayToString(ripofpubkey);


//            }


//            private void btnPubHashToAddres_Click(object sender, EventArgs e)
//            {
//                byte[] hex = ValidateAndGetHexPublicHash();
//                if (hex == null) return;

//                byte[] hex2 = new byte[21];
//                Array.Copy(hex, 0, hex2, 1, 20);

//                int cointype = 0;
//                if (Int32.TryParse(cboCoinType.Text, out cointype) == false) cointype = 0;

//                if (cboCoinType.Text == "Testnet") cointype = 111;
//                if (cboCoinType.Text == "Namecoin") cointype = 52;
//                hex2[0] = (byte)(cointype & 0xff);
//                txtBtcAddr.Text = ByteArrayToBase58Check(hex2);

//            }


//            private void btnAddressToPubHash_Click(object sender, EventArgs e)
//            {
//                byte[] hex = Base58ToByteArray(txtBtcAddr.Text);
//                if (hex == null || hex.Length != 21)
//                {
//                    int L = txtBtcAddr.Text.Length;
//                    if (L >= 33 && L <= 34)
//                    {
//                        if (MessageBox.Show("Address is not valid.  Attempt to correct?", "Invalid address", MessageBoxButtons.YesNo) == DialogResult.Yes)
//                        {
//                            CorrectBitcoinAddress();
//                            return;
//                        }
//                    }
//                    else
//                    {
//                        //MessageBox.Show("Address is not valid.");
//                    }
//                    return;
//                }
//                txtPubHash.Text = ByteArrayToString(hex, 1, 20);

//            }


//            private void btnGenerate_Click(object sender, EventArgs e)
//            {

//                ECKeyPairGenerator gen = new ECKeyPairGenerator();
//                var secureRandom = new SecureRandom();
//                var ps = Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256k1");
//                var ecParams = new ECDomainParameters(ps.Curve, ps.G, ps.N, ps.H);
//                var keyGenParam = new ECKeyGenerationParameters(ecParams, secureRandom);
//                gen.Init(keyGenParam);

//                AsymmetricCipherKeyPair kp = gen.GenerateKeyPair();

//                ECPrivateKeyParameters priv = (ECPrivateKeyParameters)kp.Private;

//                byte[] hexpriv = priv.D.ToByteArrayUnsigned();
//                txtPrivHex.Text = ByteArrayToString(hexpriv);

//                btnPrivHexToWIF_Click(null, null);
//                btnPrivToPub_Click(null, null);
//                btnPubHexToHash_Click(null, null);
//                btnPubHashToAddres_Click(null, null);

//            }


//            private void btnBlockExplorer_Click(object sender, EventArgs e)
//            {
//                /*
//                try
//                {
//                    if (cboCoinType.Text == "Testnet")
//                    {
//                        Process.Start("http://www.blockexplorer.com/testnet/address/" + txtBtcAddr.Text);
//                    }
//                    else if (cboCoinType.Text == "Namecoin")
//                    {
//                        Process.Start("http://explorer.dot-bit.org/a/" + txtBtcAddr.Text);
//                    }
//                    else
//                    {
//                        Process.Start("http://www.blockexplorer.com/address/" + txtBtcAddr.Text);
//                    }
//                }
//                catch { }*/
//            }

//            private void CorrectBitcoinAddress()
//            {
//                txtBtcAddr.Text = Correction(txtBtcAddr.Text);
//            }

//            private string Correction(string btcaddr)
//            {

//                int btcaddrlen = btcaddr.Length;
//                string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

//                for (int i = 0; i < btcaddrlen; i++)
//                {
//                    for (int j = 0; j < 58; j++)
//                    {
//                        string attempt = btcaddr.Substring(0, i) + b58.Substring(j, 1) + btcaddr.Substring(i + 1);
//                        byte[] bytes = Base58ToByteArray(attempt);
//                        if (bytes != null)
//                        {
//                            //MessageBox.Show("Correction was successful.  Try your request again.");
//                            return attempt;
//                        }
//                    }
//                }
//                return btcaddr;
//            }

//            private void CorrectWIF()
//            {
//                txtPrivWIF.Text = Correction(txtPrivWIF.Text);
//            }

//        }

//    }
//}
