using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SnowFlake
{
    /// <summary>
    /// 分布式自增演算法 - 產生唯一ID
    /// </summary>
    public static partial class SnowFlakeAlg
    {

        #region Method

        #region Mac Function 10bit

     
        private static class GetMacAddress
        {
            /// <summary>
            /// Ascii對應表
            /// </summary>
            private static Dictionary<char, int> dicAscii = new Dictionary<char, int>()
            {
                {'0' , 0},{ '1' ,1} , {'2' , 2},{ '3' ,3} , {'4' , 4},{ '5' ,5} , {'6' , 6},{ '7' ,7} , {'8' , 8},{ '9' ,9} , {'A' , 10},{ 'B' ,11} , {'C' , 12},{ 'D' ,13} , { 'E' ,14} ,{'F' , 15}
            };

            /// <summary>
            /// 取得10位元的網路卡位址
            /// </summary>
            /// <returns></returns>
            public static long GetTenBitMacAddress()
            {
                //取得網卡Libary - 取得本機器所有網路卡
                NetworkInterface[] macs = NetworkInterface.GetAllNetworkInterfaces();

                //取得電腦上的 Ethernet 的 MAC Address ，第一個抓到的實體網卡
                var result = macs.Where(o => o.NetworkInterfaceType == NetworkInterfaceType.Ethernet).FirstOrDefault();

                //沒有網卡
                if (null == result)
                {
                    return 0;
                }   //return 0L;
                else//有網卡則進行計算
                {
                    //※邏輯 ： SnowFlake 演算法取10bit ，實體網卡為 12Byte EX: E0-3F-49-4D-01-1C
                    //          取最後兩個Byte(2 * 8) 進行6bit位移，取10Bit 

                    //String -> ASCII 
                    byte[] macDecByte = System.Text.Encoding.ASCII.GetBytes(result.GetPhysicalAddress().ToString());

                    //左邊
                    int left = AscIIToInt((char)Convert.ToInt32(macDecByte[8])) * 16 + AscIIToInt((char)Convert.ToInt32(macDecByte[9])) * 1 << 8;//=>x的位元   x x x x x x x x o o o o o o o o
                    int right = AscIIToInt((char)Convert.ToInt32(macDecByte[10])) * 16 + AscIIToInt((char)Convert.ToInt32(macDecByte[11])) * 1;//=> x的位元       o o o o o o o o x x x x x x x x
                    int total = left + right;//相加

                    //保留 10 bit =>  (最大整數4095 如右邊x的部分)=> o o o o o o x x x x x x x x x x   
                    total = total >> 6;//

                    return total;
                }
            }

            /// <summary>
            /// 將AscII碼 轉為 Int
            /// </summary>
            /// <returns></returns>
            private static int AscIIToInt(char item)
            {
                int resultValue = 0;
                //取得對應 Char -> Value
                dicAscii.TryGetValue(item, out resultValue);
                //返回16進位數值 
                return resultValue;
            }
        }
        #endregion

        #region TimeSpan Milliseconds 41bit
        /// <summary>
        /// 取得時間戳 
        /// </summary>
        private static class GetTimeSpan
        {
           
            /// <summary>
            /// 回傳當前時間微秒 Long型態
            /// </summary>
            /// <returns></returns>
            public static long GetTimeSpanReturnLong()
            {
                DateTime dt = DateTime.Now;//現在時間
                DateTime ori = new DateTime(1970, 1, 1, 0, 0, 0);//起源時間
                return (long)(dt - ori).TotalMilliseconds;
            }

            /// <summary>
            /// 回傳當前時間微秒 + 1 Long 型態
            /// </summary>
            /// <returns></returns>
            public static long GetTimeSpanReturnNextSecondLong()
            {
                DateTime dt = DateTime.Now.AddMilliseconds(1);//增加1微秒
                DateTime ori = new DateTime(1970, 1, 1, 0, 0, 0);//起源時間
                return (long)(dt - ori).TotalMilliseconds;
            }

        }
        #endregion

        #region Sequence 12bit

        /// <summary>
        /// 取得序列號
        /// </summary>
        private static class GetSequence
        {
            /// <summary>
            /// 12bit 最大長度
            /// </summary>
            private static long BIT12 = 4095;

            public static bool checkSeq(long nowSeq)
            {
                var check=  (nowSeq) & BIT12;
                if (check == 0)
                    return true;
                else
                    return false;
            }

        }

        #endregion

        #endregion





    }
}
