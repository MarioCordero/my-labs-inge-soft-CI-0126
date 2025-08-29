Pasos que seguí

ENTORNO LINUX

sudo apt install dotnet-host-8.0

# O instala desde los repositorios oficiales:
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0

# Agrega .NET al PATH
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools' >> ~/.bashrc
source ~/.bashrc

# Crea el proyecto Web API
dotnet new webapi -n backend-lab-C22306

# Compilar y ejecutar
dotnet run

# Salida
mario@mario-EXT-Linux:~/Desktop/UCR/my-labs-inge-soft-CI-0126/Laboratorio4_C22306$ cd backend-lab-C22306/
mario@mario-EXT-Linux:~/Desktop/UCR/my-labs-inge-soft-CI-0126/Laboratorio4_C22306/backend-lab-C22306$ dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5172
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /home/mario/Desktop/UCR/my-labs-inge-soft-CI-0126/Laboratorio4_C22306/backend-lab-C22306
warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
      Failed to determine the https port for redirect.



# Visité
http://localhost:5172/swagger/index.html

# Nota
El template webapi básico de .NET 8 ya no incluye la carpeta Controllers por defecto ni el WeatherForecastController.cs porque ahora usa minimal APIs en el Program.cs.

# Agregué el archivo .cs a Controllers

using Microsoft.AspNetCore.Mvc;

namespace backend_lab_C22306.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hola Mundo";
        }
    }
}

