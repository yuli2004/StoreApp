using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StoreApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;

namespace StoreApp.Services
{
   public class StoreAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:42714/StoreAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.68.110:42714/StoreAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "http://localhost:44363/StoreAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:42714/Images/"; //API url when using emulator on android


        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.68.110:42714/Images/";
        //private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://10.58.55.5:42714/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44363/Images/"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static StoreAPIProxy proxy = null;

        #region create proxy
        public static StoreAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new StoreAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }

        #endregion

        #region create order
        public async Task<bool> CreateOrder(Order o)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Order>(o, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddOrder", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    bool ret = JsonSerializer.Deserialize<bool>(jsonContent, options);

                    return ret;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }
        #endregion

        #region costructor
        private StoreAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
        }
        #endregion

        #region Log In
        public async Task<User> LogInAsync(string username, string pass)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/LogIn?username={username}&pass={pass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    User u = JsonSerializer.Deserialize<User>(content, options);
                    return u;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion

        #region Buyer Register
        public async Task<Buyer> RegisterBuyerAsync(Buyer buyer)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Buyer>(buyer, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/RegisterBuyer", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Buyer ret = JsonSerializer.Deserialize<Buyer>(jsonContent, options);

                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
        }
        #endregion

        #region Seller register
        public async Task<Seller> RegisterSellerAsync(Seller seller)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Seller>(seller, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/RegisterSeller", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Seller ret = JsonSerializer.Deserialize<Seller>(jsonContent, options);

                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
        }
        #endregion

        #region Edit Profile
        public async Task<User> EditProfileAsync(User user)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<User>(user, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/EditProfile", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    User ret = JsonSerializer.Deserialize<User>(jsonContent, options);

                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
        }
        #endregion

        #region UserExistsByEmailAsync
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{this.baseUri}/UserExistsByEmail?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        #region UserExistsByUsernameAsync
        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{this.baseUri}/UserExistsByUsername?username={username}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        #region upload image
        public async Task<bool> UploadImage(Models.FileInfo fileInfo, string targetFileName)
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.Name));
                multipartFormDataContent.Add(fileContent, "file", targetFileName);
                HttpResponseMessage response = await client.PostAsync($"{this.baseUri}/UploadImage", multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        #region Get image
        public static string GetImageURL()
        {
            return DEV_ANDROID_EMULATOR_PHOTOS_URL;
        }
        #endregion

        #region create lookup tables
        public async Task<LookupTables> CreateLookUpTables()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLookups");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    LookupTables table = JsonSerializer.Deserialize<LookupTables>(content, options);
                    return table;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion 

        #region GetSearchResults
        public async Task<List<Product>> GetSearchResults(string queryString)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetSearchResults?query={queryString}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<Product> result = JsonSerializer.Deserialize<List<Product>>(content, options);
                    return result;
                }
                else
                {
                    return new List<Product>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Product>();
            }
        }
        #endregion

        #region GetSoldProducts
        public async Task<List<ProductInOrder>> GetSoldProducts()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetSoldProducts");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<ProductInOrder> result = JsonSerializer.Deserialize<List<ProductInOrder>>(content, options);
                    return result;
                }
                else
                {
                    return new List<ProductInOrder>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ProductInOrder>();
            }
        }
        #endregion

        #region Update Product
        public Task UpdateProduct(Product p)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update order
        public Task UpdateOrder(Order O)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get Buyer Orders
        //public async Task<List<Order>> GetBuyerOrders(string queryString)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetBuyerOrders?query={queryString}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            JsonSerializerOptions options = new JsonSerializerOptions
        //            {
        //                ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
        //                PropertyNameCaseInsensitive = true
        //            };
        //            string content = await response.Content.ReadAsStringAsync();
        //            List<Order> result = JsonSerializer.Deserialize<List<Order>>(content, options);
        //            return result;
        //        }
        //        else
        //        {
        //            return new List<Order>();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return new List<Order>();
        //    }
        //}
        #endregion

        #region upload product
        public async Task<Product> UploadProductAsync(Product p)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Product>(p, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UploadProduct", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    Product ret = JsonSerializer.Deserialize<Product>(jsonContent, options);

                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
        }
        #endregion

        #region Remove Product
        public async Task<bool> RemoveProduct(Product p)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Product>(p, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/RemoveProduct", content);
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    bool ret = JsonSerializer.Deserialize<bool>(jsonContent, options);

                    return ret;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }
        #endregion
    }
}
