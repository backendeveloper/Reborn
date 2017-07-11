using System.Collections.Generic;
using System.Reflection;

namespace Reborn.Web.Api.Utils.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseException
    {
        /// <summary>
        /// exception owner
        /// </summary>
        /// <returns></returns>
        public virtual string Application { get; set; }

        /// <summary>
        /// hatanın key değeri
        /// hata mesajları bir resource dan çekilecekse bu key değeri ile çekilir
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// hata mesajı
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// hatanın tipi <see cref="ExceptionType"/>        
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// hata mesajlarının key value listesi
        /// </summary>
        public virtual Dictionary<string,string> Errors { get; set; }

        /// <summary>
        /// application dan gönderilecek hata tipleri
        /// </summary>
        public enum ExceptionType : int
        {
            /// <summary>
            /// tanımsız
            /// </summary>
            Undefined = 0,
            /// <summary>
            /// validation hataları
            /// </summary>
            Validation = 1,
            /// <summary>
            /// bilgi veren hatalar
            /// </summary>
            Info = 2,
            /// <summary>
            /// kontrolsüz hatalar
            /// </summary>
            System = 3
        }
    }

    public class ExceptionModel : BaseException
    {
        public override string Application => Assembly.GetEntryAssembly().GetName().Name;
    }
}
