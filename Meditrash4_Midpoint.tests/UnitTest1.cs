using Xunit;
using Meditrash4_Midpoint;
using System.Runtime.CompilerServices;
using System;
using Xunit.Abstractions;
using MySql.Data.MySqlClient;
using TypeMock.Internal;
using TypeMock.ArrangeActAssert;
using Moq;
using Autofac;
using Autofac.Extras.Moq;

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
        

        [Fact]
        public void _Test_DeleteObject()
        {
            MySqlCommand cmd1 = new MySqlCommand();
            cmd1.CommandText = MySqlHandle.genKeySelectCond(new mysqlTables.Cathegory(16, "jip"),cmd1);
            cmd1.Prepare();
            Assert.Equal("id = 16", cmd1.CommandText); 
            /*
            Assert.Equal("uid = 0", MySqlHandle.genKeySelectCond(new mysqlTables.Department("jip")));
            Assert.Equal("uid = 147520", MySqlHandle.genKeySelectCond(new mysqlTables.ExportRecords(147520)));
            Assert.Equal("uid = 0", MySqlHandle.genKeySelectCond(new mysqlTables.Records(10,5,10)));
            */


        }


    }
}