using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TestApp
{
    public static class Dir_Anlzr_Class
    {

        public static void Analyzer(string input)
        {
            foreach (var f in Directory.GetFiles(input))
            {
                var ext = Path.GetExtension(f);
                if (ext == ".log")
                {
                    if (File.ReadAllText(f).Length > 65535)
                    {
                        File.WriteAllText(f, "");
                    }
                    else
                    {
                        File.AppendAllLines(f, new[] { "Analyzed " + DateTime.Today.ToString() });
                    }
                }
                else if (ext == ".tmp")
                {
                    File.Delete(f);
                }
                else if (ext == ".txt")
                {
                    var txt = File.ReadAllText(f);
                    if (Regex.IsMatch(f, @"e-mail\s"))
                    {
                        var pat = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

                        foreach (var line in File.ReadAllLines(f))
                        {
                            foreach (Match match in Regex.Matches(line, pat))
                            {
                                MailMessage m = new MailMessage("mymail@gmail.com", match.Value);
                                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                                smtp.Credentials = new NetworkCredential("mymail@gmail.com", "0987654321");
                                smtp.EnableSsl = true;
                                smtp.Send(m);
                            }
                        }
                    }
                }
            }

            foreach (var d in Directory.GetDirectories(input))
            {
                Analyzer(d);
            }
        }

    }
}
