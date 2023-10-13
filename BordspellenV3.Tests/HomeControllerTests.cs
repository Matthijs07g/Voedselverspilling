using BordspellenV3.Controllers;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BordspellenV3.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            Mock<IGamesRepository> mock = new Mock<IGamesRepository>();
            mock.Setup(m => m.Games).Returns((new Game[]
            {
                new Game{Id = 1, Name="test1"},
                new Game{Id = 2, Name="test2"},
            }).AsQueryable<Game>());

            HomeController controller = new HomeController(mock.Object);

            IEnumerable<Game>? result =
                (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Game>;

            Game[] gameArray = result?.ToArray() ?? Array.Empty<Game>();
            Assert.Equal("test1", gameArray[0].Name);
            Assert.Equal("test2", gameArray[1].Name);
        }
    }
}
