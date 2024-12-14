using FantasyKnightWebServer.Models;
using FantasyKnightWebServer.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FantasyKnightWebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDBContext _dBContext;
        private JwtTokenService _jwtTokenService;
        public AuthController(AppDBContext dBContext)
        {
            this._dBContext = dBContext;
            _jwtTokenService = new JwtTokenService("FantasyKnightJwtTokenSecretKey123");
        }

        [HttpPost]
        public async Task<IActionResult> Signin([FromBody] ResisterRequest resisterReq)
        {
            Console.WriteLine($"Received JSON: {JsonConvert.SerializeObject(resisterReq)}");
            if (resisterReq.UUID == null)
            {
                Console.WriteLine(resisterReq);
                Console.Write("Invalid data.");
                return BadRequest("Invalid data.");
            }

            Console.Write(resisterReq.UUID);
            var user = await _dBContext.users.FirstOrDefaultAsync(u => u.UUID == resisterReq.UUID);
            if (user != null)
                return BadRequest("bad");


            UserAccountDBData userData = new UserAccountDBData()
            {
                UUID = resisterReq.UUID,
                NicName = "test",
                CreatedAt = DateTime.UtcNow,
                LastedAt = DateTime.UtcNow
            };

            _dBContext.users.Add(userData);
            await _dBContext.SaveChangesAsync();

            string token = _jwtTokenService.GenerateJwtToken(userData.UUID);

            Console.WriteLine(token);

            return Ok(token);
        }
        [HttpGet]
        public async Task<IActionResult> Login([FromBody] AuthRequest userAccount)
        {
            if (userAccount == null || userAccount.JwtToken == null)
                return BadRequest("Invailed Token");


            string uuid = _jwtTokenService.ExtractUuidFromJwt(userAccount.JwtToken);

            var user = await _dBContext.users.FirstOrDefaultAsync(u => u.UUID == uuid);
            if (user == null)
                return BadRequest("bad");



            return Ok();
        }

    }
}
