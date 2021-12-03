using Xunit;
using Meditrash4_Midpoint;
using System.Runtime.CompilerServices;
using System;
using Xunit.Abstractions;

[assembly:InternalsVisibleTo("Meditrash4_Midpoint.tests")]

namespace Meditrash4_Midpoint.tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void Test1()
        {
            Meditrash4_Midpoint.User user = new Meditrash4_Midpoint.User("root",0,-1,"root","admin");
            _testOutputHelper.WriteLine(MySqlHandle.genSelectCommandParamQ(user));
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("Insert into User" +
                "\nValues" + MySqlHandle.genSelectCommandParamQ(user));
            MySqlHandle.fillSelectCommandParamQ(ref cmd, user);
            _testOutputHelper.WriteLine(cmd.CommandText);
        }
    }
}