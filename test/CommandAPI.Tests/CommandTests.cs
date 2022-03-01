using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something awesome",
                Platform = "xUnit",
                CommandLine = "dotnet test"
            };  
        }
        
        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //Arrange

            //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            // Given
        
            // When
            testCommand.Platform = "Platform";
        
            // Then
            Assert.Equal("Platform", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            // Given
        
            // When
            testCommand.CommandLine = "CommandLine";
        
            // Then
            Assert.Equal("CommandLine", testCommand.CommandLine);
        }
    }
}