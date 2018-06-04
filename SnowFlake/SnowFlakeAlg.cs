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
        /// <summary>
        /// 鎖住的資源對象
        /// </summary>
        private static object lockItem = new object();

        /// <summary>
        /// 上一個時間戳- 進行比較用
        /// </summary>
        private static long lastDateTimeStamp = 0L;

        /// <summary>
        /// Sequence 序列號 允許0~4095
        /// </summary>
        private static long sequence = 0L;

        public static long GetGuid()
        {
            //非同步情況下，使用Lock 確保產生唯一ID
            lock(lockItem)
            {
                long result = 0L;
                //1. 41bit
                long timelong = SnowFlakeAlg.GetTimeSpan.GetTimeSpanReturnLong();
                //2. 10bit
                long macSn = SnowFlakeAlg.GetMacAddress.GetTenBitMacAddress();
                //3. 12bit
                sequence++;

                //seq == 0 表示 同一秒內已經被排了4095次，Seq 已經用盡，切換至下一秒
                if (timelong == lastDateTimeStamp)
                {
                    if (true == SnowFlakeAlg.GetSequence.checkSeq(sequence))
                    {
                        //取得下一微秒的Long值
                        timelong = SnowFlakeAlg.GetTimeSpan.GetTimeSpanReturnNextSecondLong();
                    }
                }
                else//不同微秒下
                {
                    sequence = 0;//歸0
                }
                //紀錄本次的TimeStamp
                lastDateTimeStamp = timelong;
                
                //41bit 
                result =((timelong) << 22) | macSn << 12 | sequence;

                return result;
            }
           
        }
    }
}
