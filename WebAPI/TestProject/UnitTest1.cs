using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        private readonly DonationDBContext _context;
        [Fact]
        public async Task Test1()
        {
            
            var controller = new DCandidateController( _context);

            // Act  
            var actionResult = await controller.GetDCandidate(1);

            // Assert  
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task AddDCandidateTest()
        {
            // Arrange  
            var controller = new DCandidateController(_context);
            DCandidate dCandidate = new DCandidate
            {
                fullName = "Test Department",
            };

            // Act  
            var actionResult = await controller.PostDCandidate(dCandidate);

            // Fix: Await the task and access the `Result` property of `ActionResult<T>` to get the underlying `CreatedAtActionResult`
            var createdResult = actionResult.Result as CreatedAtActionResult;

            // Assert  
            Assert.IsAssignableFrom<CreatedAtActionResult>(createdResult);
          
           
            
        }
    }
}
