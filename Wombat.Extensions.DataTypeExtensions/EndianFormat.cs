namespace Wombat.Extensions.DataTypeExtensions
{
    /// <summary>
    /// 字节格式
    /// https://cloud.tencent.com/developer/article/1601823
    /// </summary>
    public enum EndianFormat
    {
        Native = 0,
        /// <summary>
        /// Big-Endian
        /// 大端序 ABCD
        /// </summary>
        ABCD = 1,
        /// <summary>
        /// Big-endian byte swap（大端Byte swap）
        /// 中端序 BADC, PDP-11 风格
        /// </summary>
        BADC = 2,
        /// <summary>
        /// Little-endian byte swap（小端Byte swap）
        /// 中端序 CDAB, Honeywell 316 风格
        /// </summary>
        CDAB = 3,
        /// <summary>
        /// Little-Endian
        /// 小端序 DCBA
        /// </summary>
        DCBA = 4,
    }
}
