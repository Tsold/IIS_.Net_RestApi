using Commons.Xml.Relaxng;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestService.Models;
using RestService.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace RestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private static string shema = "http://schemas.datacontract.org/2004/07/RestService.Models";
        private static string con = "SqlConnectionString";

        private  IConfiguration configuration;

        public PersonController(IConfiguration config)
        {
            configuration = config;
        }



        [HttpGet]
        public People Get()
        {
            People peps = new People(SqlRepo.GetPeople(configuration.GetConnectionString(con)).ToList());
            return peps;
        }



        private bool error = false;
        [HttpPost("xml")]
        public void Post(XmlElement xmlPerson)
        {

            try
            {
                XmlDocument doc = xmlPerson.OwnerDocument;
                doc.AppendChild(xmlPerson);
                

                doc.Schemas.Add(shema, "people.xsd");
                ValidationEventHandler validation = new ValidationEventHandler(XmlValid);

                doc.Validate(XmlValid);

                if (!error)
                {
                    Type[] knownTypes = new Type[] { typeof(Person) };
                    DataContractSerializer deserialize = new DataContractSerializer(typeof(People), knownTypes);
                    MemoryStream xmlStream = new MemoryStream();
                    doc.Save(xmlStream);
                    xmlStream.Position = 0;
                    People peoples = (People)deserialize.ReadObject(xmlStream);

                    foreach (Person p in peoples.people)
                    {
                        SqlRepo.InsertPerson(p, configuration.GetConnectionString(con));
                    }

                }
                else
                {
                    Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            catch
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                
            }



        }

        private void XmlValid(object sender, ValidationEventArgs e)
        {
            error = true;
        }



       
        [HttpPost("rng")]
        public void RngPost(XmlElement xmlPerson)
        {

            try
            {
                XmlDocument doc = xmlPerson.OwnerDocument;
                doc.AppendChild(xmlPerson);
              
              
                XmlReader instance = new XmlNodeReader(doc); 
                XmlReader schema = new XmlTextReader("people.rng");
             
                RelaxngValidatingReader vr = new RelaxngValidatingReader(instance, schema);

                bool goOn = true;
                vr.InvalidNodeFound += (vr, message) =>
                {
                    Console.WriteLine("Error: " + message);
                    goOn = false;
                    return true;
                };
                while (vr.Read());


                if (goOn)
                {
                    Type[] knownTypes = new Type[] { typeof(Person) };
                    DataContractSerializer deserialize = new DataContractSerializer(typeof(People), knownTypes);
                    MemoryStream xmlStream = new MemoryStream();
                    doc.Save(xmlStream);
                    xmlStream.Position = 0;
                    People peoples = (People)deserialize.ReadObject(xmlStream);

                    foreach(Person p in peoples.people)
                    {
                        SqlRepo.InsertPerson(p, configuration.GetConnectionString(con));
                    }
                    
                   
                }
                else
                {

                    Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
                   
              

            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                Console.WriteLine(e);

            }



        }




        [HttpGet("soap/{id}")]
        public void SoapPost(int id)
        {
            SoapServiceRef.XmlGetterServiceSoapClient service = new SoapServiceRef.XmlGetterServiceSoapClient(SoapServiceRef.XmlGetterServiceSoapClient.EndpointConfiguration.XmlGetterServiceSoap);

            var res = service.GetXmlAsync(id).Result;

            Response.WriteAsync(res.Body.GetXmlResult);

        }






        }
}
