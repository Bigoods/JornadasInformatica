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
using Leaderboard_JEI.ViewModel;

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
        [Authorize(Roles = "Admin")]
        public IActionResult Upload()
        {
            GetPontos();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Upload(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string delete = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/Ficheiros/", file.FileName);
                if (System.IO.File.Exists(delete))
                {
                    System.IO.File.Delete(delete);
                }
                _context.Participante.RemoveRange(_context.Participante.ToList());
                _context.SaveChanges();
                string destination = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/Ficheiros/", Path.GetFileName(file.FileName));
                FileStream fs = new FileStream(destination, FileMode.Create);
                file.CopyTo(fs);
                fs.Close();
                popular(file.FileName);

            }
            return RedirectToAction("Lista");
        }

        public void popular(string file)
        {
            string csvPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/Ficheiros/", file);
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
                        if (DateTime.Now.Day == 9)
                            if (j == 3)
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
        public async Task<IActionResult> Lista(string numero)
        {
            GetPontos();
            if (numero != null)
                return View(await _context.Participante.Where(n => Convert.ToString(n.Num).Contains(numero)).OrderByDescending(x => x.Pontuacao).ToListAsync());
            return View(await _context.Participante.OrderByDescending(x => x.Pontuacao).ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Listadiaria(string numero)
        {
            GetPontos();
            if (numero != null)
                return View(await _context.Participante.Where(n => Convert.ToString(n.Num).Contains(numero)).OrderByDescending(x => x.PontuacaoDiaria).ToListAsync());
            return View(await _context.Participante.OrderByDescending(x => x.PontuacaoDiaria).ToListAsync());
        }
        public IActionResult Rifas()
        {
            GetPontos();

            var perfil = _context.Perfils.FirstOrDefault(x => x.Username == User.Identity.Name);

            LisRifa c = new LisRifa();
            c.Username = perfil.Username;
            c.NumRifa1 = _context.Rifas.Where(x => x.TipoRifa == 1 && x.UserName == perfil.Username).Count();
            c.NumRifa2 = _context.Rifas.Where(x => x.TipoRifa == 2 && x.UserName == perfil.Username).Count();
            c.NumRifa3 = _context.Rifas.Where(x => x.TipoRifa == 3 && x.UserName == perfil.Username).Count();
            c.NumRifa4 = _context.Rifas.Where(x => x.TipoRifa == 4 && x.UserName == perfil.Username).Count();



            return View(c);
        }
        [HttpPost]
        public IActionResult Rifas(int primeiro, int segundo, int terceiro, int quarto)
        {
            var perfil = _context.Perfils.FirstOrDefault(x => x.Username == User.Identity.Name);

            LisRifa c = new LisRifa();
            if (perfil != null)
            {
                int soma = (primeiro + segundo + terceiro + quarto) * 10;
                if (perfil.Pontos >= soma)
                {
                    perfil.Pontos = perfil.Pontos - primeiro * 10;
                    perfil.Pontos = perfil.Pontos - segundo * 10;
                    perfil.Pontos = perfil.Pontos - terceiro * 10;
                    perfil.Pontos = perfil.Pontos - quarto * 10;

                    for (int i = 0; i < primeiro; i++)
                    {
                        Rifa newRifa = new Rifa();
                        newRifa.NumRifa = _context.Rifas.Where(x => x.TipoRifa == 1).Count() + 1;
                        newRifa.UserName = User.Identity.Name;
                        newRifa.TipoRifa = 1;

                        _context.Rifas.Add(newRifa);
                        _context.SaveChanges();
                    }
                    for (int i = 0; i < segundo; i++)
                    {
                        Rifa newRifa = new Rifa();
                        newRifa.NumRifa = _context.Rifas.Where(x => x.TipoRifa == 2).Count() + 1;
                        newRifa.UserName = User.Identity.Name;
                        newRifa.TipoRifa = 2;

                        _context.Rifas.Add(newRifa);
                        _context.SaveChanges();
                    }
                    for (int i = 0; i < terceiro; i++)
                    {
                        Rifa newRifa = new Rifa();
                        newRifa.NumRifa = _context.Rifas.Where(x => x.TipoRifa == 3).Count() + 1;
                        newRifa.UserName = User.Identity.Name;
                        newRifa.TipoRifa = 3;

                        _context.Rifas.Add(newRifa);
                        _context.SaveChanges();
                    }
                    for (int i = 0; i < quarto; i++)
                    {
                        Rifa newRifa = new Rifa();
                        newRifa.NumRifa = _context.Rifas.Where(x => x.TipoRifa == 4).Count() + 1;
                        newRifa.UserName = User.Identity.Name;
                        newRifa.TipoRifa = 4;

                        _context.Rifas.Add(newRifa);
                        _context.SaveChanges();
                    }

                    c.Username = perfil.Username;
                    c.NumRifa1 = _context.Rifas.Where(x => x.TipoRifa == 1 && x.UserName == perfil.Username).Count();
                    c.NumRifa2 = _context.Rifas.Where(x => x.TipoRifa == 2 && x.UserName == perfil.Username).Count();
                    c.NumRifa3 = _context.Rifas.Where(x => x.TipoRifa == 3 && x.UserName == perfil.Username).Count();
                    c.NumRifa4 = _context.Rifas.Where(x => x.TipoRifa == 4 && x.UserName == perfil.Username).Count();


                }
                else
                    ViewBag.Message = "Erro";
            }
            GetPontos();
            return View(c);
        }

        public void GetPontos()
        {
            if (User.Identity.IsAuthenticated)
            {
                var perfil = _context.Perfils.FirstOrDefault(x => x.Username == User.Identity.Name);
                ViewBag.Pontos = perfil.Pontos;
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DarPontos()
        {
            GetPontos();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                if (Remove > 0 && perfil.Pontos - Remove >= 0)
                    perfil.Pontos = perfil.Pontos - Remove;
                _context.Perfils.Update(perfil);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { Numero = perfil.Username });
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Details(string Numero)
        {
            GetPontos();
            return View(_context.Perfils.FirstOrDefault(x => x.Username == Numero));
        }
        [Authorize(Roles = "Admin, Client")]
        public IActionResult ListPerfis()
        {
            GetPontos();
            return View(_context.Perfils.OrderByDescending(x => x.Pontos).ToList());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ListRifas()
        {
            GetPontos();

            List<LisRifa> listFinal = new List<LisRifa>();

            List<Perfil> perfil = _context.Perfils.ToList();


            foreach (var item in perfil)
            {

                LisRifa c = new LisRifa();

                c.NumRifa1 = _context.Rifas.Where(x => x.TipoRifa == 1 && x.UserName == item.Username).Count();
                c.NumRifa2 = _context.Rifas.Where(x => x.TipoRifa == 2 && x.UserName == item.Username).Count();
                c.NumRifa3 = _context.Rifas.Where(x => x.TipoRifa == 3 && x.UserName == item.Username).Count();
                c.NumRifa4 = _context.Rifas.Where(x => x.TipoRifa == 4 && x.UserName == item.Username).Count();
                c.Username = item.Username;

                listFinal.Add(c);

            }


            return View(listFinal);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ListaIdRifa()
        {
            return View(_context.Rifas.OrderBy(x=>x.TipoRifa).ToList());
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
