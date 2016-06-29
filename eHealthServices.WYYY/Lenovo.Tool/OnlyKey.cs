using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lenovo.Tool
{
    public class OnlyKey
    {
         private const String OrderChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static long lastOrder;

        /// <summary> 生成一个新的、递增的顺序号</summary>
        /// <returns> 返回顺序号
        /// </returns>
        public static long NewOrder()
        { 
            var newOrder = DateTime.Now.Ticks >> 12;
            while (true)
            {
                var oldOrder = lastOrder;
                if (newOrder <= oldOrder)
                    newOrder = oldOrder + 1;
                if (Interlocked.CompareExchange(ref lastOrder, newOrder, oldOrder) == oldOrder)
                {
                    //成功将lastOrder修改为newOrder
                    return newOrder;
                }
                //继续下一次尝试
            }
        }

        /// <summary> 生成一个新的、递增的顺序号字符串</summary>
        /// <returns> 返回顺序号字符串，长度为10
        /// </returns>
        public static string NewStringOrder()
        {
            var order = NewOrder();
            var buffer = new char[10];
            var index = 9;
            while (order > 0 && index >= 0)
            {
                buffer[index--] = OrderChars[(int)(order % 36)];
                order = order / 36;
            }
            while (index >= 0)
                buffer[index--] = '0';
            return new string(buffer);
        }

        /// <summary> 生成一个新的、递增的顺序号字符串</summary>
        public static string NewStringOrder(int length)
        {
            if (length <= 0) return string.Empty;
            var order = NewOrder();
            var buffer = new char[length];
            var index = length - 1;
            while (order > 0 && index >= 0)
            {
                buffer[index--] = OrderChars[(int)(order % 36)];
                order = order / 36;
            }
            while (index >= 0)
                buffer[index--] = '0';
            return new string(buffer);
        }

        public static string NewStringOrder8()
        {
            return NewStringOrder(8);
        }

        /// <summary>
        /// 根据Guid生成16位的唯一标示
        /// </summary>
        /// <returns></returns>
        public static string NewLikeGuid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        private OnlyKey()
        {
        }
    }
}
