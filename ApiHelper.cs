namespace ApiHelper{



public static class GET{

    public static async Task<string> From(string url){

            //Creo la instancia de cliente para hacer la petición
            HttpClient client = new();
            //Hago la peticion
            HttpResponseMessage response = await client.GetAsync(url);
            //Hago el response Ensure
            response.EnsureSuccessStatusCode();
            //Hago un string a partir de el body de mi response
            string body = await response.Content.ReadAsStringAsync();
            
            return body;
    }
}
}