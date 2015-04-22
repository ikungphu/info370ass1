using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacultyScraper
{
    class Program
    {
        public class Faculty
        {
            private string name, university, department, phd;

            public Faculty(string n, string u, string d, string p)
            {
                name = n;
                university = u;
                department = d;
                phd = p;
            }

        }

        static void Main(string[] args)
        {
            List<String> profiles;
            profiles = getProfiles("https://www.psych.ucla.edu/faculty");

            List<Faculty> facultyList = new List<Faculty>();
            foreach (String profile in profiles)
            {
                //Console.WriteLine(profile);
                Faculty prof;
                prof = extractData(profile);
                if (prof != null)
                {
                    facultyList.Add(prof);
                }
            }

            
        }

        public static Faculty extractData(string url)
        {
            Faculty prof = null;
            string n = "n/a";
            string u = "UCLA";
            string d = "Psychology";
            string p = "n/a";

            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);

            var divTags = doc.DocumentNode.SelectNodes("//div");
            if (divTags != null)
            {
                foreach (var tag in divTags)
                {
                    if (tag.Attributes["class"] != null)
                    {
                        string attribute = tag.Attributes["class"].Value;
                        if (attribute.Contains("name"))
                        {
                            string name = tag.InnerText;
                            n = name;
                        }
                        else if (attribute.Contains("phd"))
                        {
                            string phd = tag.InnerText;
                            string[] words = phd.Split(',');
                            p = words[1];
                            Console.WriteLine(n);
                            Console.WriteLine(p);
                            prof = new Faculty(n, u, d, p);
                        }
                    }
                }
            }
            return prof;
        }

        //opens root url and gets urls for each faculty's personal web page
        public static List<String> getProfiles(string address)
        {
            //sets the target url
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(address);
            List<String> profiles = new List<String>();

            //collects all of the links on the web page
            var aTags = doc.DocumentNode.SelectNodes("//a");
            if (aTags != null)
            {
                //for every link isolate links that contain a faculty profile page and return it in a list
                foreach (var tag in aTags)
                {
                    if (tag.Attributes["href"] != null)
                    {
                        string newAddress = "https://www.psych.ucla.edu" + tag.Attributes["href"].Value;
                        if (newAddress.Contains("faculty/page/"))
                        {
                            profiles.Add(newAddress);
                        }
                    }
                }
            }
            return profiles;
        }
    }
}
