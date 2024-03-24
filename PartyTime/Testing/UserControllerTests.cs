using Xunit;
using PartyTime.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using PartyTime.Contexts;
using PartyTime.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq.EntityFrameworkCore;

namespace PartyTime.Testing
{
    public class UserControllerTests
    {
        [Fact]
        public async void GetUsers_test()
        {
            var users = new List<User>
            {
                new User(1, "john.smith", "4e0680ad9d89ae24e34335ea3de8bfa796ce3fb7cb36476db1414efd17d30890", "User", "icecream"),
                new User(2, "jane.doe", "9b1be76bce51cd3ee06ac2f580eda2aebc4fbb8836f918c1cd3c6ae9b935f7fc", "User", "4e4a87b3-3146-4834-9257-8ad6d0d6f266")
            };
            var userContextMock = new Mock<ApplicationDbContext>();
            userContextMock.Setup(x => x.Users).ReturnsDbSet(users);

            var controller = new UsersController(userContextMock.Object);

            // Act
            var result = await controller.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(users.Count(), returnedUsers.Count());

        }
    }
}
