using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestService.Models.Json;
using RestService.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private static string con = "SqlConnectionString";
        private IConfiguration configuration;

        public AlbumController(IConfiguration config)
        {
            configuration = config;
        }



        [HttpGet]
        public List<AlbumInfo> Get()
        {
            return SqlRepo.GetAlbum(configuration.GetConnectionString(con)).ToList();
        }


        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            SqlRepo.DeleteAlbum(id, configuration.GetConnectionString(con));
        }

        [HttpPost("insert")]
        public void Post(AlbumInfo album)
        {
            try
            {
               
                SqlRepo.InsertAlbum(album,configuration.GetConnectionString(con));
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            


        }



    }
}
