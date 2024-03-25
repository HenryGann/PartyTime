using Xunit;
using PartyTime.Models;
using Moq;
using PartyTime.Contexts;
using PartyTime.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq.EntityFrameworkCore;

namespace PartyTime.Testing
{
    public class UserControllerTests
    {
        private UsersController _controller;
        private List<User> users;

        public UserControllerTests() {

            users = new List<User>
            {
                new(1, "john.smith", "4e0680ad9d89ae24e34335ea3de8bfa796ce3fb7cb36476db1414efd17d30890", "User", "icecream"),
                new(2, "jane.doe", "9b1be76bce51cd3ee06ac2f580eda2aebc4fbb8836f918c1cd3c6ae9b935f7fc", "User", "4e4a87b3-3146-4834-9257-8ad6d0d6f266")
            };
            
            var userContextMock = new Mock<ApplicationDbContext>();
            userContextMock.Setup(x => x.Users).ReturnsDbSet(users);
            userContextMock.Setup(m => m.Users.FindAsync(It.IsAny<object[]>())).Returns<object[]>(async (d) => await Task.FromResult(users.FirstOrDefault()));



            userContextMock.Setup(m => m.Users.Remove(It.IsAny<User>())).Callback<User>((entity) => users.Remove(entity));

            userContextMock.Setup(x => x.SaveChangesAsync(default))
               .ReturnsAsync(1);
            _controller = new UsersController(userContextMock.Object);
        
        }
        [Fact]
        public async void GetUsers_test()
        {
            var res = await _controller.GetUsers();

            var okResult = Assert.IsType<OkObjectResult>(res);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(users.Count, returnedUsers.Count());
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContent()
        { 
            var res = await _controller.DeleteUser(1);

            var noContentResult = Assert.IsType<NoContentResult>(res);
            Assert.Equal(204, noContentResult.StatusCode);

            // Ensure the user is deleted from the list
            var deletedUser = users.FirstOrDefault(u => u.Id == 1);
            Assert.Null(deletedUser);
        }

    }
}
