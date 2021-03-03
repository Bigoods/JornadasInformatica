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
using System.Data;
using System.Security.Claims;


namespace Leaderboard_JEI.Controllers
{
    [Authorize]
    public class ParticipantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public ParticipantesController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _appEnvironment = env;
        }

        // GET: Participantes
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            GetPontos();
            return View(await _context.Participante.ToListAsync());
        }

        /*public async Task<IActionResult> UploadCSVAsync()
        {
            return View();
        }*/
        public IActionResult Upload()
        {
            GetPontos();
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Participante.RemoveRange(_context.Participante.ToList());
                string destination = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/Ficheiros/", Path.GetFileName(file.FileName));
                FileStream fs = new FileStream(destination, FileMode.Create);
                file.CopyTo(fs);
                fs.Close();
                popular();
                
            }
            return RedirectToAction("Lista");
        }

        public void popular()
        {
            string csvPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/Ficheiros/", "Jornadas - Total Final.csv");
            string csvData = System.IO.File.ReadAllText(csvPath);
            int i = 0, j = 0;
            foreach (string row in csvData.Split("\n"))
            {
                if (i >= 2)
                {
                    Participante dt = new Participante();
                    j = 0;
                    foreach (string column in row.Split(","))
                    {
                        if (j == 0)
                            dt.Num = Convert.ToInt32(column);
                        if (j == 1)
                            dt.Pontuacao = Convert.ToInt32(column);
                        if (DateTime.Now.Day==9)
                            if(j==3)
                                dt.PontuacaoDiaria = Convert.ToInt32(column);
                        if (DateTime.Now.Day == 10)
                            if (j == 5)
                                dt.PontuacaoDiaria = Convert.ToInt32(column);
                        if (DateTime.Now.Day == 11)
                            if (j == 7)
                                dt.PontuacaoDiaria = Convert.ToInt32(column);
                        j++;
                    }
                    _context.Participante.Add(dt);
                }
                i++;
            }
            _context.SaveChanges();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Lista()
        {
            GetPontos();
            return View(await _context.Participante.OrderByDescending(x=>x.Pontuacao).ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Listadiaria()
        {
            GetPontos();
            return View(await _context.Participante.OrderByDescending(x => x.PontuacaoDiaria).ToListAsync());
        }
        public IActionResult Rifas()
        {
            GetPontos();
            return View(_context.Perfils.FirstOrDefault(x => x.Username == User.Identity.Name));
        }
        [HttpPost]
        public IActionResult Rifas(int primeiro, int segundo, int terceiro, int quarto)
        {
            var perfil = _context.Perfils.FirstOrDefault(x => x.Username == User.Identity.Name);
            if (perfil != null)
            {
                int soma = (primeiro + segundo + terceiro + quarto) * 10;
                if (perfil.Pontos >= soma)
                {
                    perfil.Pontos = perfil.Pontos - primeiro * 10;
                    perfil.Pontos = perfil.Pontos - segundo * 10;
                    perfil.Pontos = perfil.Pontos - terceiro * 10;
                    perfil.Pontos = perfil.Pontos - quarto * 10;
                    perfil.Rifa1 = perfil.Rifa1 + primeiro;
                    perfil.Rifa2 = perfil.Rifa2 + segundo;
                    perfil.Rifa3 = perfil.Rifa3 + terceiro;
                    perfil.Rifa4 = perfil.Rifa4 + quarto;
                    _context.Perfils.Update(perfil);
                    _context.SaveChanges();
                }
                else
                    ViewBag.Message = "Erro";
            }
            GetPontos();
            return View(perfil);
        }

        public void GetPontos()
        {
            if(User.Identity.IsAuthenticated)
            {
                var perfil = _context.Perfils.FirstOrDefault(x => x.Username == User.Identity.Name);
                ViewBag.Pontos = perfil.Pontos;
            }
        }

        public IActionResult DarPontos()
        {
            GetPontos();
            return View();
        }
        [HttpPost]
        public IActionResult DarPontos(string UserName, int Add, int Remove)
        {
            GetPontos();
            var perfil = _context.Perfils.FirstOrDefault(x => x.Username == UserName);
            if (perfil == null)
            {
                ViewBag.Message = "Erro";
                return View();
            }
                
            else
            {
                if (Add > 0)
                    perfil.Pontos = perfil.Pontos + Add;
                if(Remove > 0 && perfil.Pontos-Remove >= 0)
                    perfil.Pontos = perfil.Pontos - Remove;
                _context.Perfils.Update(perfil);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { Numero=perfil.Username });
        }
        public IActionResult Details(string Numero)
        {
            GetPontos();
            return View(_context.Perfils.FirstOrDefault(x => x.Username == Numero));
        }

        public IActionResult ListPerfis()
        {
            GetPontos();
            return View(_context.Perfils.OrderByDescending(x => x.Pontos).ToList());
        }
        public IActionResult ListRifas()
        {
            GetPontos();
            return View(_context.Perfils.OrderByDescending(x => x.Pontos).ToList());
        }

        /*public async Task<IActionResult> UploadCSVAsync(List<IFormFile> arquivos)
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
        }*/


        public void FillDB()
        {

        }
    }
}
