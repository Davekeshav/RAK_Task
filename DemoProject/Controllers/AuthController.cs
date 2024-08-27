//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace DemoProject.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        [HttpPost]
//        [Route("generateToken")]
//        public IActionResult GenerateToken()
//        {
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("F9AB9BD8405223421C9573A9CDE83EC9E5A4A5175F0C90479526C872C7765734"));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//            new Claim(ClaimTypes.Name, "keshav")
//        };

//            var token = new JwtSecurityToken(
//                issuer: "localhost:5000",
//                audience: "localhost:5000",
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(30),
//                signingCredentials: creds);

//            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
//        }

//    }
//}
