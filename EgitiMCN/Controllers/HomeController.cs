using EgitiMCN.Models.Dto;
using EgitiMCN.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EgitiMCN.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AramizaKatilin(Basvuru basvuru)
        {
            try
            {
                var email= "s.yilmazturk@tongucakademi.com;b.yilmaz@tongucakademi.com;e.fidan@tongucbilisim.com;info@egitimcn.com,oya.gokay@synapsis.com.tr";
                var konu = "Yeni Başvuru Geldi";
                var mesaj = $@"
                            <table> <tr> <th colspan=""2"">Başvuru Bilgileri</th> </tr> 
                            <tr><td>Ad</td>  <td>{basvuru.Ad}</td> </tr>
                            <tr><td>Email</td> <td>{basvuru.Email}</td> </tr>
                            <tr><td>Hesap</td> <td>{basvuru.HesapLink}</td> </tr>
                            <tr><td>Mesaj</td> <td>{basvuru.Mesaj}</td> </tr>

                            </table>
                            ";
                ProviderManager providerManager = new ProviderManager();
                providerManager.SendMail(email,konu,mesaj);
                
                providerManager.SaveToFile(basvuru);
                
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Durum=true,
                        Mesaj = "Bilgileriniz alındı arkadaşlarımız en kısa zamanda sizinle iletişime geçecektir."
                    }
                };
            }
            catch (Exception exception)
            {

                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Durum = false,
                        Mesaj = exception.Message
                    }
                };
            }
        }


    }
}