using System;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.V1.Controllers;
using MultCo_ISD_API.V1.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Sqlite;

namespace UnitTests
{
    [TestClass]
    public class TestAPIControllers
    {
        [TestMethod]
        public void TestInMemoryDBConnection()
        {
            // Build in-mem db
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            //Assert.AreEqual(connection.State, ConnectionState.Open);

            try
            {
                // In-mem db exists only while connection is open
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                // Create schema in db
                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.SaveChanges();
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetPrograms()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.Program.Add(new Program());
                    context.SaveChanges();

                    ProgramsController controller = new ProgramsController(context);
                    var actionResult = controller.GetProgram().Result;

                    Assert.IsNotNull(actionResult);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        /* 
         * Test Methods for Services Controller
         */
        [TestMethod]
        public void TestGetServices()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    ServicesController controller = new ServicesController(context);
                    var actionResult = controller.GetServices().Result;

                    Assert.IsNotNull(actionResult);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestGetService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    ServicesController controller = new ServicesController(context);
                    var actionResult = controller.GetService(2).Result;
                    var result = actionResult as OkObjectResult;
                    var service = result.Value as ServiceV1DTO;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(200, result.StatusCode);
                    Assert.IsNotNull(service);
                    Assert.AreEqual(2, service.ServiceId);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPostService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    var serv = new Service().ToServiceV1DTO();
                    ServicesController controller = new ServicesController(context);
                    var actionResult = controller.PostService(serv).Result;
                    var result = actionResult as NoContentResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(204, result.StatusCode);
                    
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestPutService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    Service serv = new Service();
                    context.Service.Add(serv);
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    serv.ServiceName = "Panda Adoption Society";
                    var controller = new ServicesController(context);
                    var actionResult = controller.PutService(1, serv.ToServiceV1DTO()).Result;
                    var result = actionResult as NoContentResult;

                    Assert.IsNotNull(result);
                    Assert.AreEqual(204, result.StatusCode);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestDeleteService()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<InternalServicesDirectoryV1Context>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new InternalServicesDirectoryV1Context(options))
                {
                    context.Database.EnsureCreated();
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.Service.Add(new Service());
                    context.SaveChanges();

                    var controller = new ServicesController(context);
                    var actionResult = controller.DeleteService(2).Result;

                    Assert.IsNotNull(actionResult);
                    
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}