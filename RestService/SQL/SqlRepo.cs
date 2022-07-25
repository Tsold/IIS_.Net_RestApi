using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using RestService.Models;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using RestService.Models.Json;

namespace RestService.SQL
{
    public class SqlRepo
    {
       

        public static IEnumerable<Person> GetPeople(string cs)
        {
            

            DataTable tblKupci = SqlHelper.ExecuteDataset(cs, nameof(GetPeople)).Tables[0];

            foreach (DataRow row in tblKupci.Rows)
            {
                yield return new Person
                    {
                        IDPerson = (int)row[nameof(Person.IDPerson)],
                        Name = (string)row[nameof(Person.Name)],
                        Surname = (string)row[nameof(Person.Surname)],
                        Gender = (string)row[nameof(Person.Gender)],
                        PictureUrl = (string)row[nameof(Person.PictureUrl)]

                    };
            }
            

        }


        public static IEnumerable<AlbumInfo> GetAlbum(string cs)
        {


            DataTable tblKupci = SqlHelper.ExecuteDataset(cs, nameof(GetAlbum)).Tables[0];

            foreach (DataRow row in tblKupci.Rows)
            {
                yield return new AlbumInfo
                {
                    Album = (string)row["Name"],
                    Albumkey = (string)row["AlbumKey"],
                };
            }

        }



        public static void InsertPerson(Person p, string cs) => SqlHelper.ExecuteNonQuery(cs, MethodBase.GetCurrentMethod().Name, p.Name, p.Surname, p.Gender, p.PictureUrl);
        public static void InsertAlbum(AlbumInfo a, string cs) => SqlHelper.ExecuteNonQuery(cs, MethodBase.GetCurrentMethod().Name, a.Album, a.Albumkey);
        public static void DeleteAlbum(string id, string cs) => SqlHelper.ExecuteNonQuery(cs, MethodBase.GetCurrentMethod().Name,id);



    }
}
