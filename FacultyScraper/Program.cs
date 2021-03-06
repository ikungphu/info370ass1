﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacultyScraper
{
    class Program
    {
        public class Faculty
        {
            private string fname, lname, university, department, phd;

            public Faculty(string n, string l, string u, string d, string p)
            {
                fname = n;
                lname = l;
                university = u;
                department = d;
                phd = p;
            }

            public string toString()
            {
                return fname + ", " + lname + ", " + university + ", " + department + ", " + phd;
            }

        }

        static void Main(string[] args)
        {
            List<Faculty> facultyList = new List<Faculty>();

            List<String> psychProfiles;
            psychProfiles = getPsychProfiles("https://psychology.clas.asu.edu/asu_directory?title=12");

            foreach (String profile in psychProfiles)
            {
                Faculty prof;
                prof = extractPsychData(profile);
                if (prof != null)
                {
                    facultyList.Add(prof);
                }
            }



            //------------------------------------------------------------------------

            /*List<String> engProfiles;
            engProfiles = getEngProfiles("http://www.mae.ucla.edu/people/faculty");

            foreach (String profile in engProfiles)
            {
                Console.WriteLine(profile);
            } */

            List<String> psyProfiles;
            psyProfiles = getPsyProfiles("https://www.psych.ucla.edu/faculty");

            foreach (String profile in psyProfiles)
            {
                Faculty prof;
                prof = extractPsyData(profile);
                if (prof != null)
                {
                    facultyList.Add(prof);
                }
            }
            //--------------------------------------------------------------------

            List<String> lawProfiles;
            lawProfiles = getLawProfiles("https://law.ucla.edu/faculty/faculty-profiles/");

            foreach (String profile in lawProfiles)
            {
                Faculty prof;
                prof = extractLawData(profile);
                extractLawData(profile);

                if (prof != null)
                {
                    facultyList.Add(prof);
                }
            }

            foreach (String profile in lawProfiles)
            {
                Faculty prof;
                prof = extractLawData(profile);
                extractLawData(profile);

                if (prof != null)
                {
                    facultyList.Add(prof);
                }
            }

            foreach (String profile in lawProfiles)
            {
                Faculty prof;
                prof = extractLawData(profile);
                extractLawData(profile);

                if (prof != null)
                {
                    facultyList.Add(prof);
                }
            }

            exportCsv(facultyList);
            

            

            
        }



        public static void exportCsv(List<Faculty> facultyList)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\iguest\Desktop\faculty.csv"))
            {
                foreach (var faculty in facultyList)
                {
                    //Console.WriteLine(faculty.toString());
                    sw.WriteLine(faculty.toString());
                }
            }
        }

        public static Faculty extractLawData(string url)
        {
            Faculty prof = null;
            string n = "n/a";
            string l = "n/a";
            string u = "UCLA";
            string d = "Law";
            string p = "n/a";

            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);

            //extract names
            var imgs = doc.DocumentNode.SelectNodes("//img");
            foreach (var img in imgs)
            {
                if (img.Attributes["title"] != null)
                {
                    string attribute = img.Attributes["title"].Value;
                    string[] names = attribute.Split(' ');
                    n = names[0];
                    l = names[1];
                }
            }

            //extract phds
            var divTags = doc.DocumentNode.SelectNodes("//div");
            if (divTags != null)
            {
                foreach (var tag in divTags)
                {
                    if (tag.Attributes["class"] != null)
                    {
                        string attribute = tag.Attributes["class"].Value;
                        if(attribute.Contains("credential")) {
                            string content = tag.InnerText.Trim();
                            string[] split1 = content.Split(',');
                            string line = "UCLA";
                            foreach (string school in split1)
                            {
                                if (school.Contains("Ph.D"))
                                {
                                    string temp = school;
                                    string[] partition = temp.Split('.');
                                    line = partition[2].Trim();
                                    p = line;
                                    Console.WriteLine(line);
                                    prof = new Faculty(n, l, u, d, p);
                                }
                            }

                        }
                    }
                }
            }
            return prof;
        }

        public static Faculty extractPsychData(string url)
        {
            Faculty prof = null;
            string n = "n/a";
            string l = "n/a";
            string u = "ASU";
            string d = "Psychology";
            string p = "n/a";

            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);

            var h1 = doc.DocumentNode.SelectNodes("//h1");
            if (h1 != null)
            {
                foreach (var tag in h1)
                {
                    var names = tag.InnerText;
                    string[] words = names.Split(' ');
                    n = words[0];
                    l = words[1];
                }
            }
            

            var pTags = doc.DocumentNode.SelectNodes("//p");
            if (pTags != null)
            {
                foreach (var tag in pTags)
                {
                    if (tag.InnerText != null)
                    {
                        string attribute = tag.InnerText;
                        if (attribute.Contains("Ph.D"))
                        {
                            string name = attribute.Trim();
                            string[] words = name.Split(' ');
                            p = words[1].Trim();
                            prof = new Faculty(n, l, u, d, p);
                        }
                    }
                }
            }
            return prof;
        }

        // collects all necessary fields from each faculty profile and returns a faculty object
        public static Faculty extractPsyData(string url)
        {
            Faculty prof = null;
            string n = "n/a";
            string l = "n/a";
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
                            string name = tag.InnerText.Trim();
                            string[] words = name.Split(' ');
                            if (words.Length > 2)
                            {
                                n = words[0];
                                l = words[2];
                            }
                            else
                            {
                                n = words[0];
                                l = words[1];
                            }
                            
                        }
                        else if (attribute.Contains("phd"))
                        {
                            string phd = tag.InnerText;
                            string[] words = phd.Split(',');
                            p = words[1].Trim();
                            //Console.WriteLine(n);
                            //Console.WriteLine(p);
                            prof = new Faculty(n, l, u, d, p);
                        }
                    }
                }
            }
            return prof;
        }

        //opens asu psychology root url and gets urls for each faculty's web page
        public static List<String> getPsychProfiles(string address)
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
                        string newAddress = "https://psychology.clas.asu.edu" + tag.Attributes["href"].Value;
                        if (newAddress.Contains("faculty/"))
                        {
                            profiles.Add(newAddress);
                        }
                    }
                }
            }
            return profiles;
        }

        //opens ucla psychology root url and gets urls for each faculty's personal web page
        public static List<String> getPsyProfiles(string address)
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

        public static List<String> getLawProfiles(string address)
        {
            //sets the target url
            HtmlWeb hw = new HtmlWeb(); 
            HtmlDocument doc = hw.Load(address);
            List<String> profiles = new List<String>();

            var aTags = doc.DocumentNode.SelectNodes("//a");
            if (aTags != null)
            {
                foreach (var tag in aTags)
                {
                    if (tag.Attributes["href"] != null)
                    {
                        string newAddress = "https://law.ucla.edu" + tag.Attributes["href"].Value;
                        if (newAddress.Contains("faculty-profiles/") && !profiles.Contains(newAddress))
                        {
                            profiles.Add(newAddress);
                        }
                    }
                }
            }
            return profiles;
        }

        public static List<String> getEngProfiles(string address)
        {
            //sets the target url
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(address);
            List<String> profiles = new List<String>();

            var aTags = doc.DocumentNode.SelectNodes("//a");
            if (aTags != null)
            {
                foreach (var tag in aTags)
                {
                    if (tag.Attributes["href"] != null)
                    {
                        string newAddress = "https://www.mae.ucla.edu/people/faculty/" + tag.Attributes["href"].Value;
                        if (newAddress.Contains("faculty-profiles/") && !profiles.Contains(newAddress))
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
