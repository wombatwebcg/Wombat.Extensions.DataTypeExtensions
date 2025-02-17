using System;
using System.Linq;
using System.Text;
using Xunit;
using Wombat.Extensions.DataTypeExtensions;

namespace Wombat.Extensions.DataTypeExtensionsTest
{
    public class DataTypeExtensionsTests
    {
        // 测试数据
        public readonly byte[] _sampleBytes = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        public readonly byte[] _fullRangeBytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();


    }
    public class BasicConversions : DataTypeExtensionsTests
    {
        [Fact]
        public void ToString_WithUTF8_ReturnsCorrectString()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello 你好");
            Assert.Equal("Hello 你好", bytes.ToString(Encoding.UTF8));
        }

        [Fact]
        public void ToBase64String_ConvertsCorrectly()
        {
            var bytes = new byte[] { 0x01, 0x02, 0x03 };
            Assert.Equal("AQID", bytes.ToBase64String());
        }

        [Fact]
        public void ToBinString_SingleByte_CorrectFormat()
        {
            Assert.Equal("00000000", ((byte)0x00).ToBinString());
            Assert.Equal("11111111", ((byte)0xFF).ToBinString());
        }

        [Fact]
        public void ToHexString_ProducesLowercase()
        {
            Assert.Equal("123456789abcdef0", _sampleBytes.ToHexString());
            Assert.Equal("f0", ((byte)0xF0).ToHexString());
        }
    }

    public class NumericConversions : DataTypeExtensionsTests
    {
        [Theory]
        [InlineData(0x1234, false, new byte[] { 0x34, 0x12 })] // 小端
        [InlineData(0x1234, true, new byte[] { 0x12, 0x34 })]  // 大端
        public void Int16_ConversionWithEndian(short value, bool reverse, byte[] expected)
        {
            Assert.Equal(expected, value.ToByte(reverse));
        }

        [Theory]
        [InlineData(0x12345678, EndianFormat.ABCD, new byte[] { 0x12, 0x34,0x56,0x78 })]
        [InlineData(0x12345678, EndianFormat.BADC, new byte[] { 0x34,0x12,0x78,0x56 })]
        [InlineData(0x12345678, EndianFormat.CDAB, new byte[] { 0x56, 0x78, 0x12, 0x34 })]
        [InlineData(0x12345678, EndianFormat.DCBA, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
        public void Int32_EndianHandling(int value, EndianFormat format, byte[] expected)
        {
            var pp = BitConverter.GetBytes(value);
            var pp1 = value.ToByte(format);

            Assert.Equal(expected, value.ToByte(format));
        }

        [Theory]
        [InlineData(0x1234567831323334, EndianFormat.ABCD, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x31, 0x32, 0x33, 0x34 })]
        [InlineData(0x1234567831323334, EndianFormat.BADC, new byte[] { 0x34, 0x12, 0x78, 0x56, 0x32,0x31, 0x34, 0x33})]
        [InlineData(0x1234567831323334, EndianFormat.CDAB, new byte[] { 0x56, 0x78, 0x12, 0x34, 0x33, 0x34, 0x31, 0x32 })]
        [InlineData(0x1234567831323334, EndianFormat.DCBA, new byte[] { 0x78, 0x56, 0x34, 0x12, 0x34, 0x33, 0x32, 0x31})]

        public void Int64_EndianHandling(long value, EndianFormat format, byte[] expected)
        {
            Assert.Equal(expected, value.ToByte(format));
        }

        [Fact]
        public void Float_ConversionAccuracy()
        {
            float original = 123.456f;
            var bytes = original.ToByte(EndianFormat.ABCD);
            Assert.Equal(original, bytes.ToFloat());
        }

        [Fact]
        public void Double_ConversionAccuracy()
        {
            double original = double.MaxValue-1;
            var pp = BitConverter.GetBytes(original);
            var bytes = original.ToByte(EndianFormat.DCBA);
            Assert.Equal(original, bytes.ToDouble(format: EndianFormat.DCBA));
        }

    }

    public class EndiannessTests : DataTypeExtensionsTests
    {
        [Theory]
        [InlineData(EndianFormat.ABCD, 0x12345678)]
        [InlineData(EndianFormat.BADC, 0x34127856)]
        [InlineData(EndianFormat.CDAB, 0x56781234)]
        [InlineData(EndianFormat.DCBA, 0x78563412)] 
        public void Int32_MultiFormatSupport(EndianFormat format, int expected)
        {
            byte[] bytes = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            var value = bytes.ToInt32(format: format);
            Assert.Equal(expected, value);
        }


        [Theory]
        [InlineData(EndianFormat.ABCD, 0x1234567831323334)]
        [InlineData(EndianFormat.BADC, 0x3412785632313433)]
        [InlineData(EndianFormat.CDAB, 0x5678123433343132)]
        [InlineData(EndianFormat.DCBA, 0x7856341234333231)]
        public void Int64_MultiFormatSupport(EndianFormat format, long expected)
        {
            byte[] bytes = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x31, 0x32, 0x33, 0x34 };
            var value = bytes.ToInt64(format: format);
            var pp = BitConverter.GetBytes(value);
            var pp1 = BitConverter.GetBytes(expected);

            Assert.Equal(expected, value);
        }
    }

    public class ArrayConversions : DataTypeExtensionsTests
    {
        [Fact]
        public void BoolArray_ParsingBits()
        {
            byte[] input = new byte[] { 0b10101010 }; // 二进制 10101010
            bool[] expected = new[] { true, false, true, false, true, false, true, false };
            Assert.Equal(expected, input.ToBoolArray(0, 8));
        }

        [Fact]
        public void DoubleArray_PrecisionHandling()
        {
            double[] values = { 1.23, 4.56 };
            var bytes = values.ToByte(EndianFormat.ABCD);
            Assert.Equal(values, bytes.ToDouble(0, bytes.Length));
        }
    }

    public class ExceptionHandling : DataTypeExtensionsTests
    {
        //[Fact]
        //public void InvalidLength_ThrowsArgumentException()
        //{
        //    byte[] invalid = new byte[5];
        //    Assert.Throws<ArgumentException>(() => invalid.ToInt32());
        //}

        [Fact]
        public void OutOfRangeIndex_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _sampleBytes.ToInt32(10));
        }
    }
}
