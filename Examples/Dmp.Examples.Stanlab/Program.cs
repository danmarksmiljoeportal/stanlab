using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dmp.Examples.Stanlab
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        public async static Task Run()
        {
            string stanlabAddress = "https://stanlab.test.miljoeportal.dk";
            string autority = "https://log-in.test.miljoeportal.dk/runtime/oauth2";

            string clientId = "** insert client id **";
            string clientSecret = "** insert client secret **";

            var httpClient = new HttpClient();

            WriteConsoleInfo("Connection to PULS Stanlab 2.0");
            var client = new StanlabClient(stanlabAddress, httpClient);
            await client.AddAuthorization(autority, clientId, clientSecret);

            WriteConsoleInfo("Submitting sample");
            var sampleId = await SubmitTestSample(client);

            WriteConsoleInfo("Submitting analyses to the sample");
            await SubmitTestAnalyses(client, sampleId);

            WriteConsoleInfo("Retrieving the submitted sample");
            var sample = await GetTestSample(client, sampleId);

            WriteConsoleInfo("Deleting the sample");
            await DeleteTestSample(client, sampleId);

            WriteConsoleInfo("Success");

            Console.ReadLine();
        }

        private static void WriteConsoleInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }
        
        public static async Task<Guid> SubmitTestSample(StanlabClient client)
        {
            var command = new SubmitSampleRequest
            {
                ObservationFacilityId = "396b071b-6488-485a-8d9e-bcff45079d56", // Renseanlæg: Hirtshals - Afløb
                Location = "",

                Purpose = 1, // Egenkontrol
                Type = 2, // Mængdeproportional prøve   
                Matrix = 1, // Vand

                Label = "Prøve nr. 1 af 12",

                Applicant = new Actor
                {
                    Company = "DK29809577",
                    Person = ""
                },
                
                Sampling = new Sampling
                {
                    Sampler = new Actor
                    {
                        Company = "DK29809577",
                        Person = "Christian Lykke"
                    },
                    
                    Method = 0, // Ikke oplyst
                    Equipment = 0, // Ikke oplyst
                    Remarks = "Prøven var meget uklar i forhold til hvad den plejer at være...",

                    SamplingStarted = DateTime.Now,
                    SamplingEnded = DateTime.Now.AddDays(1)
                },
                
                Measurements = new ObservableCollection<Measurement>
                {
                    new Measurement
                    {
                        Parameter = 1155, // Vandføring
                        Unit = 33, // m3/d
                        Method = 0, // Ikke oplyst
                        Accredited = false,

                        Attribute = null,
                        Value = 12000,

                        Time = DateTime.Now,
                        Executor = new Actor
                        {
                            Company = "DK29809577",
                            Person = "Christian Lykke"
                        }
                    }
                },

                Observations = new ObservableCollection<Observation>
                {
                    new Observation
                    {
                        Parameter = 1894, // Farve, vandfase
                        StancodeList = 1060, // Farve (kodeliste)
                        Stancode = 3 // Gullig
                    }
                }
            };

            return await client.SubmitSampleAsync("puls", command);
        }

        public static async Task SubmitTestAnalyses(StanlabClient client, Guid sampleId)
        {
            var command = new SubmitAnalysesRequest
            {
                SampleId = sampleId,

                Applicant = new Actor
                {
                    Company = "DK29809577",
                    Person = ""
                },

                Laboratory = new Actor
                {
                    Company = "DK29809577",
                    Person = "Christian Lykke"
                },

                Reference = "Rapport nr. 2018-0299123-22",

                Registered = DateTime.Now,
                Finished = DateTime.Now,

                StorageTemperature = 0,

                Remarks = "Detektionsgrænse hævet for Total-P pga. interferens",

                Analyses = new ObservableCollection<Analysis>
                {
                    new Analysis
                    {
                        Parameter = 176, // BI5 modif. Biokemisk iltforbrug,modifificeret 5 døgn
                        Fraction = 1, // Ingen separering

                        Unit = 1, // mg/L
                        Method = 0, // Ikke oplyst

                        Attribute = null,
                        Value = 10,

                        Laboratory = new Actor
                        {
                            Company = "DK29809577",
                            Person = "Christian Lykke"
                        },
                        Accredited = true,

                        DetectionLimit = 0.1,

                        RelativeUncertainty = 0.2,
                        AbsoluteUncertainty = 2
                    }
                }
            };

            await client.SubmitAnalysesAsync("puls", command);
        }

        public static async Task<Sample> GetTestSample(StanlabClient client, Guid sampleId)
        {
            return await client.GetSampleAsync("puls", sampleId);
        }

        public static async Task DeleteTestSample(StanlabClient client, Guid sampleId)
        {
            await client.DeleteSampleAsync("puls", sampleId);
        }
    }
}