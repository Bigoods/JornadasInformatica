using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Leaderboard_JEI.Data;
using Leaderboard_JEI.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Leaderboard_JEI.Controllers
{
    [Authorize]
    public class ParticipantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        IHostingEnvironment _appEnvironment;

        public ParticipantesController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _appEnvironment = env;
        }

        // GET: Participantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Participante.ToListAsync());
        }

        public async Task<IActionResult> UploadCSVAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadCSVAsync(List<IFormFile> arquivos)
        {
            long tamanhoArquivos = arquivos.Sum(f => f.Length);
            // caminho completo do arquivo na localização temporária
            var caminhoArquivo = Path.GetTempFileName();

            // processa os arquivo enviados
            //percorre a lista de arquivos selecionados
            foreach (var arquivo in arquivos)
            {
                //verifica se existem arquivos 
                if (arquivo == null || arquivo.Length == 0)
                {
                    //retorna a viewdata com erro
                    ViewData["Erro"] = "Error: Arquivo(s) não selecionado(s)";
                    return View(ViewData);
                }
                
                // < define a pasta onde vamos salvar os arquivos >
                string pasta = "Ficheiros";
                // Define um nome para o arquivo enviado incluindo o sufixo obtido de milesegundos
                string nomeArquivo = arquivo.FileName;

                //< obtém o caminho físico da pasta wwwroot >
                string caminho_WebRoot = _appEnvironment.WebRootPath;
                // monta o caminho onde vamos salvar o arquivo : 
                // ~\wwwroot\Arquivos\Arquivos_Usuario\Recebidos
                string caminhoDestinoArquivo = caminho_WebRoot + "\\" + pasta + "\\" + nomeArquivo;

                //copia o arquivo para o local de destino original
                using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }
            }

            FillDB();

            return View();
        }


        public void FillDB()
        {

        }
    }
}
