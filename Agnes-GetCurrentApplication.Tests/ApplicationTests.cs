using System;
using Xunit;
using GetCurrentApplication;

namespace Agnes_GetCurrentApplication.Tests
{
    public class ApplicationTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            string expected = "Hello world";

            // Act
            string actual = Worker.test();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
