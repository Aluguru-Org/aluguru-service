using System;
using Xunit;

namespace Mubbi.Marketplace.Rent.Domain.Tests
{
    public class VoucherTests
    {
        [Fact]
        public void CreateVoucher_WhenEmptyCode_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Voucher(string.Empty, EVoucherType.Value, 10, 1, DateTime.UtcNow.AddDays(30)));
        }

        [Theory]
        [InlineData(0.00)]
        [InlineData(-10.00)]
        [InlineData(-1000.00)]
        public void CreateVoucher_WhenVoucherTypeValue_AndDiscountSmallerOrEqualThanZero_ShouldThrowDomainException(decimal discount)
        {
            Assert.Throws<Exception>(() => new Voucher("fake_code", EVoucherType.Value, discount, 1, DateTime.UtcNow.AddDays(30)));
        }

        [Theory]
        [InlineData(0.00)]
        [InlineData(-10.00)]
        [InlineData(-1000.00)]
        [InlineData(1000.00)]
        public void CreateVoucher_WhenPercentTypeValue_AndDiscountSmallerOrEqualThanZero_OrDiscountGreaterThan100_ShouldThrowDomainException(decimal discount)
        {
            Assert.Throws<Exception>(() => new Voucher("fake_code", EVoucherType.Percent, discount, 1, DateTime.UtcNow.AddDays(30)));
        }

        [Fact]
        public void CreateVoucher_WhenAmountSmallerThanOne_ShouldThrowDomainException()
        {
            Assert.Throws<Exception>(() => new Voucher("fake_code", EVoucherType.Value, 10, 0, DateTime.UtcNow.AddDays(30)));
        }
    }
}
