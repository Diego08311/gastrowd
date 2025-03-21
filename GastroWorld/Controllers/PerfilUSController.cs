using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GastroWorld.Models.Model;
namespace GastroWorld.Controllers
{
    public class PerfilUSController : Controller
    {
        public IActionResult perfil()
        {
            return View("perfil");
        }
    }
}


    

