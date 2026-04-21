using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using G4_SC601_KN_Proyecto.EntityFramework;

namespace G4_SC601_KN_Proyecto.Controllers
{
    public class GeminiController : Controller
    {
        //llave
        private readonly string _apiKey = "AIzaSyCg6NGIK6yekb90i41Lk5hVr8ePPYIAa-g";
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Consultar(string pregunta)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            try
            {
                string contextoInventario = "";


                using (var db = new ProyectoDBEntities())
                {
                    var fechaLimite = DateTime.Now.AddMonths(6);
                    var vencimientosProximos = db.stock.Where(s => s.lote.fecha_vencimiento <= fechaLimite).Count();
                    var totalStock = db.stock.Sum(s => (int?)s.cantidad) ?? 0;

                    contextoInventario = $"Eres un asistente experto en logística para la empresa Terumo Neuro. " +
                                         $"Usa estos datos exactos de la base de datos para responder de forma profesional y concisa (máximo 3 líneas): " +
                                         $"Actualmente hay {totalStock} unidades en stock total. " +
                                         $"Alerta: Hay {vencimientosProximos} lotes que vencerán en menos de 6 meses. ";
                }


                string promptFinal = contextoInventario + "Pregunta del usuario: " + pregunta;


                using (var client = new HttpClient())
                {

                    var requestBody = new
                    {
                        contents = new[] {
                            new { parts = new[] { new { text = promptFinal } } }
                        }
                    };

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");


                    var response = await client.PostAsync(_apiUrl + _apiKey, jsonContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                        string textoIA = jsonResponse.candidates[0].content.parts[0].text;
                        textoIA = textoIA.Replace("**", "");
                        return Json(new { respuesta = textoIA });
                    }
                    else
                    {
                        return Json(new { respuesta = $"Error detallado ({response.StatusCode}): {responseString}" });
                    }
                }
            }
            catch (Exception ex)
            {
                Models.BitacoraHelper.GuardarError(ex, "GeminiController - API Real");
                return Json(new { respuesta = "Hubo un error de conexión con la IA. Consulta la bitácora." });
            }
        }
    }
}