using System;
using UserService.Utilities.Logic;
using Xunit;

namespace UserService.Test.Utilities
{
    public class PasswordUtilityTest : TestBase
    {
        [Fact]
        public void NullPasswordHashing()
        {
            Assert.Throws<ArgumentException>(() => new PasswordUtility().ToHash(null));
        }

        [Fact]
        public void EmptyPasswordHashing()
        {
            Assert.Throws<ArgumentException>(() => new PasswordUtility().ToHash(""));
        }

        [Fact]
        public void NotEmptyPasswordHashing()
        {
            string hash = new PasswordUtility().ToHash("password");

            Assert.False(string.IsNullOrEmpty(hash));
        }
    }
}