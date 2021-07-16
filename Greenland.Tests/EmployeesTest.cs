using Greenland.API.DB;
using Greenland.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Greenland.Tests
{
    public class EmployeesTest
    {
        [Fact]
        public void TestGetEmployeeByUsername()
        {
            var context = new EmployeesDB();

            var username = "joaquin.phoenix";
            var id = 13;
            Employees emp = context.GetEmployeeByUsername(username).GetAwaiter().GetResult();
            Assert.Equal(id, emp.IdEmployee);

            username = "filip";
            Assert.Null(context.GetEmployeeByUsername(username).GetAwaiter().GetResult());
        }

        [Fact]
        public void TestGetEmployeeById()
        {
            var context = new EmployeesDB();

            var username = "joaquin.phoenix";
            var id = 13;
            var emp = context.GetEmployeeById(id);
            Thread.Sleep(1000);
            // test efikasnosti, ne eka se emp nego se nastavlja izvoditi dalje, a  kada se dode do emp.GetAwaiter()
            // prieka se ako eventualno nije gotov dohvat rez  = 1.7 sec
            Assert.Equal(username, emp.GetAwaiter().GetResult().Username);

            id = 999999;
            Assert.Null(context.GetEmployeeById(id).GetAwaiter().GetResult());
        }

        [Fact]
        public void TestGetEmployeesBYIDteam()
        {
            var context = new EmployeesDB();

            var IDteam = 5;
            IEnumerable<int> ids_expected = new List<int>(new int[] { 26, 49, 51, 53, 54 });
            IEnumerable<int> ids_resut = context.GetEmployeesBYIDteam(IDteam).GetAwaiter().GetResult();
            Assert.Equal(ids_expected, ids_resut);

            IDteam = 999999;
            Assert.Empty(context.GetEmployeesBYIDteam(IDteam).GetAwaiter().GetResult().ToList());
        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateEmployee()
        {
            var context = new EmployeesDB();

            var username = "joaquin.phoenix";
            var id = 13;
            Employees emp = context.GetEmployeeById(id).GetAwaiter().GetResult();
            emp.Username = "filip";
            await context.UpdateEmployee(emp);
            Employees result_emp = context.GetEmployeeById(id).GetAwaiter().GetResult();
            Assert.Equal("filip", result_emp.Username);
            emp.Username = username;
            await context.UpdateEmployee(emp);
            result_emp = context.GetEmployeeById(id).GetAwaiter().GetResult();
            Assert.Equal(username, result_emp.Username);

            emp.IdEmployee = 999;
            await Assert.ThrowsAsync<System.InvalidOperationException>(() => context.UpdateEmployee(emp));
        }

        [Fact]
        public void TestGetEmployeeByMail()
        {
            var context = new EmployeesDB();

            var mail = "joaquin.phoenix@greenland.agile.com";
            var id = 13;
            Employees emp = context.GetEmployeeByMail(mail).GetAwaiter().GetResult();
            Assert.Equal(id, emp.IdEmployee);

            mail = "nepostojeci mail";
            Assert.Null(context.GetEmployeeByMail(mail).GetAwaiter().GetResult());
        }

        [Fact]
        public void TestGetEmployeeByIdSync()
        {
            var context = new EmployeesDB();

            var username = "joaquin.phoenix";
            var id = 13;
            Employees emp = context.GetEmployeeByIdSync(id);
            Thread.Sleep(1000);
            // test efikasnosti, eka se na emp pa se nastavlja rez  = 1.9 sec
            Assert.Equal(username, emp.Username);

            id = 999999;
            Assert.Null(context.GetEmployeeByIdSync(id));
        }

        [Fact]
        public void GetEmployeeByMailSync()
        {
            var context = new EmployeesDB();

            var mail = "joaquin.phoenix@greenland.agile.com";
            var id = 13;
            Employees emp = context.GetEmployeeByMailSync(mail);
            Assert.Equal(id, emp.IdEmployee);

            mail = "nepostojeci mail";
            Assert.Null(context.GetEmployeeByMailSync(mail));
        }
    }

}
