using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Dmp.Stanlab.References.StanlabClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait();
        }

        public async static Task Run()
        {
            string stanlabAddress = "https://stanlab-api.test.miljoeportal.dk";
            string authority = "https://log-in.test.miljoeportal.dk/runtime/oauth2";

            string clientId = "** insert client id **";
            string clientSecret = "** insert client secret **";

            var client = new HttpClient();

            string accessToken = await GetAccessToken(client, authority, clientId, clientSecret);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            WriteConsoleInfo("Connecting to Stanlab API");
            var stanlab = new StanlabApi(stanlabAddress, client);

            WriteConsoleInfo("Submitting sample");
            var sampleId = await SubmitSample(stanlab);

            WriteConsoleInfo("Submitting laboratory test to the sample");
            await SubmitLaboratoryTest(stanlab, sampleId);

            WriteConsoleInfo("Retrieving the submitted sample");
            var sample = await GetSample(stanlab, sampleId);

            WriteConsoleInfo("Deleting the sample");
            await DeleteSample(stanlab, sampleId);

            WriteConsoleInfo("Success");

            Console.ReadLine();
        }

        private static void WriteConsoleInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }

        public static async Task<Guid> SubmitSample(StanlabApi stanlab)
        {
            var command = new SubmitSampleRequest
            {
                ObservationFacilityId = "396b071b-6488-485a-8d9e-bcff45079d56", // Renseanlæg: Hirtshals - Afløb
                Location = "",
                
                Purpose = 1, // Egenkontrol
                Type = 2, // Mængdeproportional prøve   
                Matrix = 1, // Vand

                Label = "Prøve nr. 1 af 12",

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
                
                Measurements = new List<Measurement>
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
                        Performer = "Miljøstyrelsens Laboratorium - DANAK 556"
                    }
                },

                Observations = new List<Observation>
                {
                    new Observation
                    {
                        Parameter = 1894, // Farve, vandfase
                        StancodeList = 1060, // Farve (kodeliste)
                        Stancode = 3, // Gullig
                        Performer = "Miljøstyrelsens Laboratorium - DANAK 556"
                    }
                }
            };

            return await stanlab.SubmitSampleAsync("puls", command);
        }

        public static async Task SubmitLaboratoryTest(StanlabApi stanlab, Guid sampleId)
        {
            var command = new SubmitLaboratoryTestRequest
            {
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

                Analyses = new List<AnalysisResult>
                {
                    new AnalysisResult
                    {
                        Parameter = 176, // BI5 modif. Biokemisk iltforbrug,modifificeret 5 døgn
                        Fraction = 1, // Total
                        
                        Unit = 1, // mg/L
                        Method = 0, // Ikke oplyst

                        Attribute = null,
                        Value = 10,
                        
                        Laboratory = "Miljøstyrelsens Laboratorium - DANAK 556",
                        Accredited = true,

                        Time = DateTime.Now,

                        DetectionLimit = 0.1,
                        QuantificationLimit = 0.3,
                        
                        RelativeUncertainty = 0.2,
                        AbsoluteUncertainty = 2
                    }
                },
                
                Appendix = null
            };

            await stanlab.SubmitLaboratoryTestAsync("puls", sampleId, command);
        }

        public static async Task<Sample> GetSample(StanlabApi stanlab, Guid sampleId)
        {
            return await stanlab.GetSampleAsync("puls", sampleId);
        }

        public static async Task DeleteSample(StanlabApi stanlab, Guid sampleId)
        {
            await stanlab.DeleteSampleAsync("puls", sampleId);
        }

        private static async Task<string> GetAccessToken(HttpClient httpClient, string authority, string clientId, string clientSecret)
        {
            var tokenEndpoint = await GetTokenEndpoint(authority);

            var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = tokenEndpoint,

                ClientId = clientId,
                ClientSecret = clientSecret,

                Scope = "stanlab_read stanlab_write"
            });

            if (response.IsError)
            {
                throw new UnauthorizedAccessException(response.Error);
            }

            return response.AccessToken;
        }

        private static async Task<string> GetTokenEndpoint(string authority)
        {
            var client = new HttpClient();

            var discoveryDocument = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = authority,
                Policy = new DiscoveryPolicy
                {
                    ValidateIssuerName = false,
                    ValidateEndpoints = false
                }
            });

            if (discoveryDocument.IsError)
            {
                throw new AuthenticationException(discoveryDocument.Error);
            }

            return discoveryDocument.TokenEndpoint;
        }
    }
}