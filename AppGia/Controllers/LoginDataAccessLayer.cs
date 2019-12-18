using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace AppGia.Controllers
{
    public class LoginDataAccessLayer
    {

        public Response Login(Login lg)
        {
            string dominio = "infogia";
            string path = "LDAP://ServerOmnisys/CN=users, DC=Infogia, DC=local";
            string domainAndUsername = dominio + @"\" + lg.UserName;
            DirectoryEntry entry = new DirectoryEntry(path,
            domainAndUsername, lg.Password);
            try
            {
                DirectorySearcher dir = new DirectorySearcher(entry);
                dir.ToString();
                SearchResult result = dir.FindOne();
                return new Response { MESSAGE = true };

            }
            catch (DirectoryServicesCOMException cex)
            {
                return new Response { MESSAGE = false };
            }

            catch (Exception ex)
            {
                return new Response { MESSAGE = false };
            }
        }
      }
    }

