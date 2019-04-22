using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using EgitiMCN.Models.Dto;
using Newtonsoft.Json;

namespace EgitiMCN.Models.Managers
{
    public class ProviderManager
    {
        public bool SendMail(string email, string konu, string mesaj)
        {
            try
            {
                MailMessage ePosta = new MailMessage
                {
                    IsBodyHtml = true,
                    From = new MailAddress("basvuru@egitimcn.com", "EğitiMCN")
                };
                var emails = email.Split(';');
                foreach (var e in emails)
                {
                    if (!string.IsNullOrEmpty(e))
                        ePosta.To.Add(e);
                }
                ePosta.Subject = konu;
                ePosta.Body = mesaj;


                SmtpClient smtp = new SmtpClient("smtp.yandex.com.tr");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Timeout = 10000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("basvuru@egitimcn.com", "Bsv/*2493");
                smtp.Send(ePosta);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveToFile(Basvuru basvuru)
        {
            var yol = HttpContext.Current.Server.MapPath("~/Applications/data.json");
            var basvurular = Read(yol);
            if (basvurular == null) basvurular = new BasvuruListesi();
            if (basvurular.Basvurular == null) basvurular.Basvurular = new List<Basvuru>();
            basvurular.Basvurular.Add(basvuru);
            Write(basvurular, yol);
        }

        public BasvuruListesi Read(string filePath)
        {
            if(!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            return JsonConvert.DeserializeObject<BasvuruListesi>(File.ReadAllText(filePath));
        }

        public void Write(BasvuruListesi model, string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(model));
        }
    }
}